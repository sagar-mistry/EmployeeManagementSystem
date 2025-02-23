This coding exercise is given by RBC interviewer as a part of 3rd round.

Assume that there is a table that will provide the details of employees (employee number, employee name, hourly rate and hours worked).

You need to build ASP.net core REST API that will expose end-points to store employee information, update employee information (by employee number), delete employee information (by employee number) and also fetch information (search all employees that match a name, specific employee by employee number, and as well as fetch employee information by employee number).
The table needs to have the following information recorded - Employee number, Employee name, hourly rate, hours worked and total pay (calculated value = hourly rate * hours worked).


The API should allow you to present the employee details from the table in pages with a configurable page size.
That means, if the page size is configured as 50, I should see the employee details 50 at a time.

Components -

1.       An ASP.NET Web API that can insert/update/delete/read employee details to/from the DB and provide employee details when requested

Expectations -
1. Solution(s) should use the new Asp.Net 8.0/9.0 framework
2. Database should be SQL Server. You can use the free edition on your PC for your demo.
3. The service(s) and solutions have to be checked into GitHub, and available for my team for review.
4. Sufficient logging and documentation on the service(s), setup/installation steps etc. carries extra points.
5. We expect your code to be close to production quality.
6. There are no specific security requirements for the problem above.
