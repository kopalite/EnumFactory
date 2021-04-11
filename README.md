# EnumFactory

Simple Factory Pattern, naming convention based, with 1-to-1 mapping between custom enum values and named instances.
Variant classes being instantiated by factory can remain clean: all you need is to follow the naming convention.
Reduces boilerplate factory implementation code to one-liners. Improves open/closed principle, 
by closing off the factory DI registration code: it stays the same forever,
even when new variant classes and enumeration values are added along the way.

# QuickStart example:

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

}

public class LocalOrderService : IOrderService
{

}

public class AmazonOrderService : IOrderService
{

}
```

- use IServiceCollection extension method to register IEnumFactory<OrderType, IOrderService>:

```
services.AddEnumFactory<OrderType, IOrderService>();
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