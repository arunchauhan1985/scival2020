CREATE TABLE `sci_countrycodes` (
  `COUNTRYID` bigint NOT NULL,
  `NAME` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `LCODE` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `SCODE` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `REGION` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `SUBREGION` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  PRIMARY KEY (`COUNTRYID`)
);
