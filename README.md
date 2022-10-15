# MoveNetBarracuda

I tried running Google's MoveNet model with Unity Barracuda.

Please don't expect it work in the near future.
I figured out that it's not useful at the moment because the model depends on the TopK operator
-- The GPU Barracuda runtime can't handle it in a performant way (quite slow).
I'll resume the project when the TopK issue is resolved.
