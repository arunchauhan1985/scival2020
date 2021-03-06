CREATE TABLE `sci_language_detail` (
  `TRAN_ID` decimal(20,0) NOT NULL,
  `SCIVAL_ID` decimal(20,0) DEFAULT NULL,
  `COLUMN_DESC` blob,
  `COLUMN_ID` decimal(20,0) DEFAULT NULL,
  `MODULEID` decimal(20,0) DEFAULT NULL,
  `LANGUAGE_ID` decimal(20,0) DEFAULT NULL,
  `TRAN_TYPE_ID` decimal(20,0) DEFAULT NULL,
  `CREATED_DATE` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`TRAN_ID`)
);
