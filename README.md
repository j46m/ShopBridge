# ShopBridge

1. Documentation via Swagger
	- /swagger/index.html

2. Authentication via claims and JWT
	- /api/Authentication/token
	
3. RESTful URLs and actions
		
		GET /product
		GET /product/{id}
		GET /product/{name}

		POST /product
		PATCH /product/{id}
		PUT /product/{id}

		DELETE /product/{id} 

4. Query parameters for advanced filtering, sorting and searching.

5. Versioning via URL.

6. Returning errors with HTTP Codes.

8. Data integration with EF Core.

7. Healthchecks.

		Will check availability of SQL Server connection with a string connection, 
		parameter can be passed to constructor.

		/health

9. Logging.

10. Unit testing with NUnit.