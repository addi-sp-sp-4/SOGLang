grammar Language ;
import Lexer ;


comparator: COMPARATOR;

single: SINGLE;
nullval: NULL;


vector3: (single COMMA single COMMA single) ;

string : STRING;

bool : BOOL ;

datatype : nullval | string | bool | single | vector3 | list | dict;

numbertype: single | vector3 ;

alltype: function | comparison | math | datatype;

keyvaluepair: alltype COLON alltype ;

dict: lcurlybracket (keyvaluepair (',' keyvaluepair)* )? rcurlybracket ;

list: lbracket (alltype (',' alltype)* )? rbracket ;

lparen: LPAREN;
rparen: RPAREN;

lbracket: LBRACKET ;
rbracket: RBRACKET;

lcurlybracket: LCURLYBRACKET ;
rcurlybracket: RCURLYBRACKET ;

fname: FNAME ;

comparison: (

	((function | datatype | math ) comparator (function | datatype | math)) |
	((function | datatype | math ) comparator lparen (function | datatype | math) rparen ) |
	( lparen (function | datatype | math ) rparen comparator (function | datatype | math)) |
	( lparen (function | datatype | math ) rparen comparator lparen (function | datatype | math) rparen )


);

expression: (
	
		(extension | function | comparison | math | datatype) |
	lparen (extension | function | comparison | math | datatype) rparen



) ;

function: fname '(' (expression ((COMMA expression)+)?)* ')' ;

operator: OPERATOR;


/* Seems like a shitty solution, doesn't it? */
/* You're absolutely right */
/* This is so we can nest math formations */

math: (

	(function | numbertype) (operator (function | numbertype))+ 					|
	
	lparen (function | numbertype | math) rparen (operator (function | numbertype))+ 		|
	
	(function | numbertype ) (operator lparen (function | numbertype | math) rparen )+ 	|
	
	lparen (function | numbertype | math) rparen ( operator lparen (function | numbertype | math) rparen )+

) ; 

extension: (
                                                                                                
	(alltype) extender (alltype) |
	
	lparen (extension | alltype) rparen extender (alltype) |
	
	(alltype) extender lparen (extension | alltype) rparen |
	
	lparen (extension | alltype) rparen extender lparen (alltype) rparen 
	

) ; 

extender: EXTENDER;

command: expression EOF; 
