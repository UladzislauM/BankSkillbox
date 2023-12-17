CREATE TABLE IF NOT EXISTS clients
(
	id			BIGINT NOT NULL,
    first_name 	CHARACTER VARYING(100),
    last_name	CHARACTER VARYING(100), 
    history 	CHARACTER VARYING(2000), 
    prestige 	INT, 
    status 		CHARACTER VARYING(30) DEFAULT 'General' NOT null,
    PRIMARY KEY (id)
)

CREATE TABLE IF NOT EXISTS scores
(
	id 					BIGINT NOT NULL, 
    balance 			DECIMAL(25, 10), 
    percent 			DECIMAL(10, 10), 
    date_score 			DATE, 
    is_capitalization 	BOOLEAN DEFAULT FALSE NOT NULL, 
    is_money 			BOOLEAN DEFAULT FALSE NOT NULL, 
    deadline 			DATE, 
    date_last_dividends DATE, 
    client_id 			BIGINT NOT NULL, 
    score_type 			CHARACTER VARYING(30) NOT NULL, 
    is_active 			BOOLEAN DEFAULT FALSE NOT NULL,
    PRIMARY KEY (id),
	FOREIGN KEY (client_id) REFERENCES clients (id)
)
