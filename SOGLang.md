# SOGLang

SOGLang (Sea Of Greed Language) is a very simple interpreted semi-programming language with very primitive logic.

## So, why does this exist?

SOGLang's primary use is in the quest system. We do **not** want to write every quest manually. Using SOGLang we can write quests without writing 'real' code. 

For example, you can give up a condition like *"playerpos() == 'programmers_test_1'"*. currentmap is a soglang function that returns the current map the player is on. If the player is on the map *programmers_test_1*, this query will return true. 

(In the quest system it advances to the next part of the quest when this is the case, but that's not a feature of SOGLang itself).

## Functionality

* Native Types
	* [String](#string)
	* [Single](#single)
	* [Bool](#bool)
	* [Vector3](#vector3)
	* [Null](#null)

* Logic
	* [Arithemetic](#arithmetic)
	* [Comparison](#comparison)
	* [Logical statements](#logical-statements)
	* [Functions](#functions)


## Types


### String

String type. A string is declared like this:


`'string'`



### Single

Single / Float type. A single is declared like this:

```
10
-4
-8.52362
```

### Bool

Bool type. A bool is declared like this:

```
true
false
```

Additionally, the only possible values a bool can have are of course "true" and "false".

### Vector3


A Vector3 is a list of 3 [Singles](#single). It is declared like this:

```
2, 1, -4.214
(5, -421, 5236.414)
```

### null

Null value. Null is declared like this

`null`


## Logic


### Arithmetic

You can do math with [Singles](#single) and [Vector3s](#vector3). String concatenation is not possible as of now.

Many combinations with a vector3 as right hand value don't work properly, like `1 ^ (3, -4.5, 4) // Doesn't work`

Standard precedences apply.
 
You can use parentheses to prioritize.


#### Operators
* **^**
* **&#42;**
* **\\**
* **-**
* **+**

#### Examples

```
10.5 + 23 // Returns 34.5
(10, 20, 30) * (40, 50, 60) // Returns (400, 1000, 1800)
10 * (10 + 3) / 5 ^ 2 // Returns 5.2
```


### Comparison

With the `==` and `!=` comparators you can compare every type. All others only work on a combination of 2 [singles](#single) or 2 [vector3s](#vector3)

SOGLang treats every value that is 

1. not `0`
2. not `(0, 0, 0)`
3. not `''` (empty string)
4. not `false`

as true. 

#### Comparators

* **==**
* **!=**
* **<**
* **>**
* **<=**
* **>=**

#### Examples

```
5 + 10 == 15 // Returns true
5 + 10 < 15 // Returns false
currentmap() == 'programmers_test_1' // returns true if the player's current map is 'programmers_test_1`, else returns false
```


### Logical statements

Only logical AND and logical OR operators are supported. If you want to use logical NOT use the [comparator](#comparators) !=.

There is **no** support for short circuiting. SOGLang is quite dumb and evaluates both the left- and righthand value before comparing.

#### Logical operators

* **&&**
* **||**


#### Examples

```
10 < 20 && true // returns true
400 > 423 || 1 // returns true (due to 1 being a truthy value)

```


### Functions


Functions are the biggest part of SOGLang because they let you actually do something in the game.

Functions can accept (optionally typed) parameters. All functions return a value.

#### List of functions

**NOTE: THIS MIGHT BE OUTDATED**

* **[vector3](#vector3) playerpos()**
	* Returns the current position of the player

* **[string](#string) currentmap()**
	* Returns the current map

* **[vector3](#vector3) setplayerpos([vector3](#vector3) pos)** 
	* **Parameters**
    	* pos: The coords the player position should be set to
	* Sets the player's position to parameter 'pos'
	* Returns the position of the player after the player has been teleported (same values as pos)	


* **[vector3](#vector3) ~setplayerpos([vector3](#vector3) pos)** 
    * **Parameters**
    	* pos: the coordinates you want the player to be teleported to, relatively

    * Sets the player's position to parameter 'pos' relatively to the current player position
    * Returns the player position after the player has been teleported

* **[vector3](#vector3) setmap([string](#string) map)**
	* **Parameters**
		* map: the map the player should be teleported to
	* Sets the map of the player
	* Returns the position of the player after the player has been teleported

* **[vector3](#vector3) vector3([single](#single) par0, [single](#single) par1, [single](#single) par2)**
	* **Parameters**
		* par0: first single of the vector3
		* par1: second single of the vector3
		* par2: third single of the vector3
	* Returns a vector3 composed of the 3 arguments given

* **[bool](#bool) debug(&#42;parameters)**
	* **Parameters**
		* parameters: potentionally infinite list of arguments. They can be of different types

	* Debug.Logs all of the parameters 
	* Returns true (Always!)

* **[bool](#bool) setvar([string](#string) varname, value)**
	* **Parameters**
		* varname: name of the variable
		* value: the value the var should contain. This can be every type

	* Sets variable internally (It's just a Dictionary<string, object> in the code)
	* Returns true (Always!)


* **value getvar([string](#string) varname)**
	* **Parameters**
		* varname: the name of the variable previously set

	* Returns the value the variable 'varname' is set to. This can be every value.
