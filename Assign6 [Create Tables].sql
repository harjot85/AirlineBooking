 
 CREATE TABLE Airport (
  iata			char(3) primary key,
  airport_name  char(30) not null Unique,
  city			char(20) not null 
 )

CREATE TABLE Flight (
  flight_code		char(6) Primary Key,
  distance			int		NOT NULL,
  departure_iata	char(3) NOT NULL,
  arrival_iata		char(3) NOT NULL,
  FOREIGN KEY (departure_iata) REFERENCES Airport(iata) ON DELETE NO ACTION ON UPDATE NO ACTION,
  FOREIGN KEY (arrival_iata) REFERENCES Airport(iata) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT different_arriv_depart CHECK (arrival_iata <> departure_iata)
)
 
CREATE TABLE Flight_Instance (
  flight_code		char(6),	--part of compound primary key
  departs			date,		--part of compound primary key
  gate				char(3),
  aircraft_id		tinyint,
  Primary Key (flight_code, departs),
  FOREIGN KEY (flight_code) REFERENCES Flight(flight_code) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT flight_once_a_day UNIQUE (departs, aircraft_id) 
)
  
CREATE TABLE Passenger (
  passenger_id		int		 primary key,
  first_name 		char(20) not null,
  last_name 		char(20) not null,
  miles 			int		 not null
)
 
CREATE TABLE Flies (
  flight_code	char (6), --part of compound primary key
  departs		date,	  --part of compound primary key
  passenger_id  int		  --part of compound primary key
  PRIMARY KEY (flight_code, departs, passenger_id),
  FOREIGN KEY (flight_code, departs) REFERENCES Flight_Instance(flight_code, departs) ON DELETE NO ACTION ON UPDATE CASCADE,
  FOREIGN KEY (passenger_id)		 REFERENCES Passenger(passenger_id)				  ON DELETE NO ACTION ON UPDATE NO ACTION
)