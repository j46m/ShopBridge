# ShopBridge

1. Authentication via claims and JWT
	- /api/Authentication/token
	
2. RESTful URLs and actions
		
		GET /product
		GET /product/{id}
		GET /product/{name}

		POST /product
		PATCH /product/{id}
		PUT /product/{id}

		DELETE /product/{id} 

3. Query parameters for advanced filtering, sorting and searching.

4. Versioning via URL.

5. Returning errors with HTTP Codes.

6. Data integration with EF Core.

7. Healthchecks.

		Will check availability of SQL Server connection with a string connection, 
		parameter can be passed to constructor.

		/health

8. Logging.

9. Unit testing with NUnit.