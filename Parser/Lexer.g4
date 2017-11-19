lexer grammar Lexer;

WS : (' ' | '\n') -> skip;


COMMA: ',' ;

SINGLE: '-'?[0-9]+('.'[0-9]+)? ;

NULL: 'null';

COMPARATOR : (


	'<' 	|
	'>' 	|
	'>=' 	|
	'<='	|
	'=='	|
	'!='

) ;

OPERATOR : (


	'+' |
	'-' |
	'*' |
	'/' |
	'%' |
	'^'

) ;

EXTENDER : (

	'&&' |
	'||'

) ;

LPAREN: '(' ;
RPAREN: ')' ;

LBRACKET: '[' ;
RBRACKET: ']' ;

LCURLYBRACKET: '{' ;
RCURLYBRACKET: '}' ;

COLON: ':' ;


BOOL : (

	'true' |
	'false'

) ;


STRING : '"' [ -!#-~]* '"';

FNAME : [a-zA-Z~_] ([a-zA-Z0-9_]+)? ;

ERRORCHAR : . ;