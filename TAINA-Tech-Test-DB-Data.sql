Declare @firstname varchar (30)
Declare @lastname varchar (40)
Declare @gender varchar (6)
Declare @emailAddress varchar (200)
Declare @phone varchar (30)

Declare @personCount int
SET @personCount=1

WHILE @personCount <= 10
BEGIN
   SET @firstName = 'firstname' + CAST(@personCount AS VARCHAR);
   SET @lastname = 'lastname' + CAST(@personCount AS VARCHAR);
   SET @emailAddress = 'emailAddress' + CAST(@personCount AS VARCHAR) + '@test.com';
   SET @firstName = 'firstname' + CAST(@personCount AS VARCHAR);
   
    IF @personCount % 2 = 0  
    BEGIN   
        SET @gender = 'male';
    END
    ELSE
        SET @gender = 'female';
   
    DECLARE @RETURN bigint
    DECLARE @Upper bigint;
    DECLARE @Lower bigint;
    DECLARE @Random bigint;
    SET @Lower = 7000000000;
    SET @Upper = 7999999999;
    SET @RETURN= (ROUND(((@Upper - @Lower -1) * @Random + @Lower), 0));
	DECLARE @rnd1 bigint;  
	SELECT @rnd1= floor(rand()*(7999999999-7000000000+1)) + 7000000000;

    IF @personCount % 3 = 0  
    BEGIN   
        SET @phone = '+44' + CAST(@rnd1 AS VARCHAR);
    END
    ELSE
        SET @phone = '';
	
   INSERT INTO People (Firstname, LastName, Gender, EmailAddress, PhoneNumber) VALUES (@firstName, @lastname, @gender, @emailAddress, @phone)
   SET @personCount = @personCount + 1;
END;

