# ShopBridge

1. Authentication via claims and JWT
		
		/api/Authentication/token
	
2. RESTful URLs and actions (TODO: filter by user)
		
		GET /products
		GET /products/{id}
		GET /products/{name}

		POST /products
		PATCH /products/{id}
		PUT /products/{id}

		DELETE /products/{id} 


3. (TODO) Query parameters for advanced filtering, sorting and searching.
			
		GET /products?state=open
		GET /products?sort=priority

4. Versioning via URL.

5. Returning errors with HTTP Codes.

6. Data integration with EF Core.

7. Health checks.

		Checks availability of SQL Server connection with a string connection, 
		other parameters can be passed to constructor.

		/health

8. Logging.

9. Unit testing with NUnit.