# EnumFactory

- Simple Factory Pattern, convention based, with 1-1 mapping between enum values and named instances. 
- Variant classes being instantiated by factory can remain clean: just follow the proper naming.
- Reduces boilerplate code by having factory implementation and DI registration together in one-liner.
- Adding new variant classes and enum values is pure extension (in line with open/closed principle).
- DI friendly: injectable factory, no service-locator anti-pattern (in line with .NET DI).
- Lifecycle friendly: scoped, transient and singleton lifecycles are options for all elements.
- Variant classes can implement a common interface or inherit from common (abstract) class. 
- No reflection after the setup: types map is cached for each specific factory type.


# QuickStart example

- add any enumeration describing the variant classes that factory should pick up:

```
public enum OrderType 
{ 
	LocalOrder, 
	
	AmazonOrder 
} 
```

- add the abstraction for objects that will be constructed by the factory, and some implementations. 
  Abstraction can be interface or (abstract) class. in this case, it's the IOrderService interface
  and all it's implementations. Note: you can have as many implementations as you want,
  but there must be one for each enum value, as per naming convention (others are ignored).
  
- mapped implementation class names must start with the related enum value 
  and end with last word from abstraction name (case sensitive). 
  In this example, for **LocalOrder** enum value and **IOrderService** interface 
  we should have **LocalOrderService** class, and so on.

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

- use IServiceCollection extension method to register IFactory<OrderType, IOrderService>:

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
	private readonly IFactory<OrderType, IOrderService> _factory;

	public OrderController(IFactory<OrderType, IOrderService> factory)
	{
	    _factory = factory;
	}
	
	[HttpPut]
	public async Task UpdateOrder([FromBody]Order order)
	{
	    //for OrderType.LocalOrder, you'll get LocalOrderService.
	    var service = _factory.GetService(order.OrderType);
		
	    service.UpdateOrder(order); 
	}
}
```