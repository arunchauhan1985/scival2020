CREATE FUNCTION `string_splitter`(
  str text,
  delim varchar(255),
  pos int) RETURNS text CHARSET utf8
BEGIN

return replace(substring_index(str, delim, pos), concat(substring_index(str, delim, pos - 1), delim), '');

END;
