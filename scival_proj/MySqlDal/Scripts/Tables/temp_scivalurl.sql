CREATE TABLE `temp_scivalurl` (
  `Id` int GENERATED ALWAYS AS (0) STORED NOT NULL,
  `URL` varchar(4000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  PRIMARY KEY (`Id`)
);
