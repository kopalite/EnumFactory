# EnumFactory

- Simple Factory Pattern, convention based, with 1-1 mapping between enum values and named instances. 
- Variant classes being instantiated by factory can remain clean: just follow the proper naming.
- Reduces boilerplate code by having factory implementation and DI registration together in one-liner.
- Adding new variant classes and enumeration values can be done seamlessly as pure extension.
- Supports scoped, transient and singleton lifecycles for factory and variant classes.
- Variant classes can implement a common interface or inherit from common (abstract) class. 

# QuickStart example

- add any enumeration describing the variant classes that factory should pick up:

```
public enum OrderType 
{ 
	LocalOrder, 
	
	AmazonOrder 
} 
```

- add interface I(Some)Suffix and implementations with class names following the format (EnumValue)Suffix. 
  Suffix can be anything (Service, Manager, Reader etc.) as long as it's unique for the interface and all implementations. 

```
public interface IOrderService
{
	Task UpdateOrder(Order order);
}

public class LocalOrderService : IOrderService
{
	public Task UpdateOrder(Order order) { } 
}

public class AmazonOrderService : IOrderService
{
	public Task UpdateOrder(Order order) { } 
}
```

- use IServiceCollection extension method to register IEnumFactory<OrderType, IOrderService>:

```
services.AddEnumFactory<OrderType, IOrderService>();
```

- default lifecycle is Scoped, but you can chose other:

```
services.AddEnumFactory<OrderType, IOrderService>(Lifecycle.Singleton);
```

- inject factory where needed and enjoy IOrderService services! It's that simple :)

```
public class OrderController : Controller
{
	private readonly IEnumFactory<OrderType, IOrderService> _factory;

	public OrderController(IEnumFactory<OrderType, IOrderService> factory)
	{
	    _factory = factory;
	}
	
	[HttpPost]
	public async Task UpdateOrder([FromBody]Order order)
	{
	    //if order.OrderType is OrderType.Local, you will get LocalOrderService instance.
		
	    var service = _factory.GetService(order.OrderType);
		
	    service.UpdateOrder(order); 
	}
}
```