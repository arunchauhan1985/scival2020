CREATE TABLE `award_tables` (
  `NAME` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `SEQUENCE` bigint NOT NULL,
  `KEY` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `PAGENAME` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `MAPTABLE` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `FLAG` bigint NOT NULL,
  PRIMARY KEY (`KEY`)
);
