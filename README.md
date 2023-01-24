# lc-cli

A cli application to help solve and understand lambda calculus

## Features:
Basic input and interpretation

Basic solving, not fully tested

Library functions allowing you to define saved functions and use them later

## Examples

```
^> (^x.x y) e

// Will reduce to
 ~> (e y)
```

```
(sugared)
^> (^xy.y x) e a

// Will reduce to
 ~> (a e)
```

### Smart Solver

This new solving method alpharenames all variables programatically then will check all possible reductions and reduce the one with biggest impact (the one with the least ammount of total variables)

Repeating until no more reductions can be done

Library functions (deprecated)

```
^> TRUE=(^t.(^f.t))

// Then

^> TRUE a b

// Will reduce to
~> ((^t.(^f.t)) a b)
~> ((^f.a) b)
~> (a)
```
