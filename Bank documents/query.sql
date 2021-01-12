select * from Transactions;

select * from Accounts;

select * from Users where FirstName = 'Viriato';

UPDATE Accounts
SET IsActive = 1;

select definition,name
 from sys.check_constraints

 ALTER TABLE Transactions
DROP CONSTRAINT CK__Transactio__Type__01142BA1;

select * from Users;

ALTER TABLE Transactions
ADD CONSTRAINT CHK_TransType CHECK (Type IN('Deposit', 'Withdrawal', 'Transfer', 'Payment', 'Monthly Fee', 'Interest', 'Dividents'));

CREATE TRIGGER trg_UpdateTimeEntry
ON dbo.TimeEntry
AFTER UPDATE
AS
    UPDATE dbo.TimeEntry
    SET ModDate = GETDATE()
    WHERE ID IN (SELECT DISTINCT ID FROM Inserted)

insert into Logins (Username, Password, UserTypeId, UserId)
values ('Aristoteles', 'qwerty123', 3,12);


ALTER TABLE Accounts
ADD InterestFeeDate AS DATEADD(month, (DATEDIFF(month, OpenDate, GETDATE())+1), OpenDate);

SELECT OpenDate + DATEDIFF(mm, OpenDate, GETDATE()) +1

SELECT DATEDIFF(day, '2020-12-15', GETDATE())+30;
select DATEADD(day, DATEDIFF(day, '2020-12-15', GETDATE())+30, '2020/12/15');

ALTER TABLE Accounts
DROP COLUMN InterestFeeDate;

select DATEDIFF(day, OpenDate, GETDATE()) as bbb from Accounts;
OpenDate + GetDate() + 30 - OpenDate

select GETDATE() + 30;

get day of openDate


