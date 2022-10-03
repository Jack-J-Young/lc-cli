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

Library functions

```
^> TRUE=(^t.(^f.t))

// Then

^> TRUE a b

// Will reduce to
~> ((^t.(^f.t)) a b)
~> ((^f.a) b)
~> (a)
```

## TODO:

Proper UI

Input and output using args
