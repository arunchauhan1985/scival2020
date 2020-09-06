CREATE TABLE `temptable_subregion` (
  `COUNTRYNAME` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `ALPHA_2` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `ALPHA_3` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `REGION` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `SUBREGION` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  PRIMARY KEY (`REGION`)
);
