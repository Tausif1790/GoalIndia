dotnet run: Manually runs the application. Restart required for code changes.
dotnet watch: Automatically reruns the application when code changes are detected, ideal for development.

// using ISpecifiction pattern with Generic repository

// Specifiction 
instead of creating 100 repositories we can create 1 generic repo and use use for all controllers
and if we need any specific filter, sort(by name, type, etc), then we can create respective Specifiction classes 
like "ProductSpecification" class where it needed (BrandSpecification, TypeSpecification)

//projection ==> the process of changing an object into a new form, often with only the properties that will be used

//[FromQuery] ==> // so giving controller a hint that this is Query Params from request (see in postmat there is one tab called "Params") not a obect from request body