
--1. Deleted Passenger Trigger

CREATE TRIGGER Deleted_Passenger_Trigg ON PASSENGER
	FOR DELETE 
	AS
	BEGIN
		DECLARE @R_COUNT INT
		SET     @R_COUNT = (SELECT COUNT(*) FROM Flies F WHERE F.passenger_id IN (SELECT D.PASSENGER_ID FROM DELETED D)) 
			IF @R_COUNT > 0
			BEGIN
				ROLLBACK TRAN
				PRINT ('PERMISSION DENIED. ' + CAST (@R_COUNT AS VARCHAR(10)) + ' RECORD(S) EXIST.')	
			END
			ELSE
				PRINT CAST (@R_COUNT AS VARCHAR(3)) + ' RECORDS EXIST. RECORDS DELETED!'
END
/*
DROP TRIGGER RESTRICT_DELETE
DROP TRIGGER UPDATE_MILES
SELECT p.Passenger_ID FROM Passenger p WHERE P.passenger_id in (SELECT passenger_id FROM FLIES)
DELETE FROM Passenger WHERE passenger_id IN (10839, 11696, 13874)
DELETE FROM Passenger WHERE passenger_id IN (25159 , 13874)
SELECT * FROM Passenger WHERE passenger_id = 13874
*/

----------------------------------------------------------------------------------------
--2. Miles Update Trigger

			
CREATE /*ALTER*/ TRIGGER Miles_Update_Trigg ON FLIES 
 AFTER INSERT, DELETE, UPDATE
	AS 
	BEGIN 
	DECLARE @NUMBER_OF_DELETED INT
		SET @NUMBER_OF_DELETED = (SELECT COUNT(Passenger_id) FROM DELETED)
			IF @NUMBER_OF_DELETED > 0
				BEGIN
					UPDATE Passenger SET miles = Del.Fetch_Miles FROM
						(SELECT P.Passenger_id, P.Miles - SUM(F.Distance) AS Fetch_Miles FROM Deleted D 
								INNER JOIN Passenger P ON D.Passenger_id = P.Passenger_id 
								INNER JOIN Flight F ON D.Flight_code = F.Flight_code GROUP BY P.Passenger_id, P.Miles) AS Del
							WHERE Passenger.Passenger_id = Del.Passenger_id
				END	
			ELSE
				BEGIN
					UPDATE Passenger SET Miles = Ins.newMiles 	FROM
						(SELECT P.Passenger_id, P.Miles + SUM(F.Distance) AS NewMiles FROM Inserted I
								INNER JOIN Passenger P ON I.Passenger_id = P.Passenger_id
								INNER JOIN Flight F ON I.Flight_code = F.Flight_code GROUP BY P.Passenger_id, p.Miles) AS Ins
							WHERE Passenger.Passenger_id = Ins.Passenger_id
				END	
				PRINT 'Miles Updated.'
	END

/*	
drop trigger Update_miles
SELECT * FROM FLIES	WHERE passenger_id = 13423
SELECT * FROM Passenger 
SELECT * FROM Flight
SELECT * FROM Flight_Instance
UPDATE Flies SET flight_code = 'JA240', departs = '2016-11-29' WHERE passenger_id = 13423 AND flight_code = 'JA220'
INSERT INTO Flies VALUES ('JA220','2016-11-28' ,13423)
delete from Flies where passenger_id = 13423 AND flight_code = 'JA240'
*/

---------------------------------------------------------------------

