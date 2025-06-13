flags for the standalone .exe:

-vlr=5 line grouping precision, 5 is default, bigger is less precise
-gt=1.5 word grouping precision, 1.5 is default, bigger is more tollerant to big spaces
-dt=true delimiter tuples, true is default, turn strings with a splitter char into key value pairs
-td=: delimiter char, colon is default, to use for making tuples.
-tt=true titled tuples, true is default. if it finds undelimited pairs, it'll take the first row as the field names for the json. switch off and see if u like it.
-pp=true json indentation, true is default.
