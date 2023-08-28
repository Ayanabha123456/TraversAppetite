[<img src="https://img.shields.io/badge/Unity-Game Engine-important.svg?logo=unity">](<LINK>)
[<img src="https://img.shields.io/badge/CSharp-Scripting-important.svg?logo=csharp">](<LINK>)

<h1 align="center" style="font-size:60px;">TraversAppetite</h1>

It is a game-based learning tool developed using Unity and C# to teach traversal algorithms, namely First In First Out (FIFO), Last In First Out (LIFO), Breadth First Search (BFS) and Depth First Search (DFS). This repository contains the main game C# scripts, including the tests as well. In addition to that, it also contains the sprites and the backgrounds used for the game. All the other settings and tweaks were done in Unity Editor. The assets that are used in this game were obtained from [Unity Asset Store](https://assetstore.unity.com/), [Scenario](https://www.scenario.com/) and [Google Images](https://www.google.com/imghp?hl=en&tab=ri&ogbl). Edits were done with Photoshop.

# Technologies
<img src="https://images.contentstack.io/v3/assets/blt08c1239a7bff8ff5/bltdff1a2920dd347a5/63f5068a97790d11728d0a6d/U_Logo_Small_black.svg" width="50">

# Prerequistes
* Download [Unity Game Engine](https://unity.com/download)
* Install [Visual Studio](https://visualstudio.microsoft.com/)
* Import [AltTester SDk](https://alttester.com/docs/sdk/latest/pages/get-started.html#import-alttester-package-in-unity-editor) in Unity if you want to run the system tests

# Game Assets
* [Player character](https://assetstore.unity.com/packages/2d/characters/2d-tactics-character-animated-117477)
<br>
<img src="https://assetstorev1-prd-cdn.unity3d.com/key-image/071b3ed3-7efd-498c-829e-d1c6cbc3a13c.webp" width="300">

* [UI Elements 1](https://assetstore.unity.com/packages/2d/gui/icons/simple-ui-icons-147101)
<br>
<img src="https://assetstorev1-prd-cdn.unity3d.com/package-screenshot/9c5eb3e4-38d3-42f0-aa99-392a9ef1b698.webp" width="300">

* [UI Elements 2](https://assetstore.unity.com/packages/2d/gui/icons/basic-icons-139575)
<br>
<img src="https://assetstorev1-prd-cdn.unity3d.com/key-image/8e19d5fc-95f0-4b30-9e6a-4f6575ce814d.webp" width="300">

* [Ingredients](https://assetstore.unity.com/packages/2d/gui/icons/food-icons-pack-70018)
<br>
<img src="https://assetstorev1-prd-cdn.unity3d.com/key-image/ec6bb0e0-60d1-4ba1-aced-128bddedcf67.webp" width="300">

* [Queue/Stack cell](https://unitycodemonkey.com/video.php?v=BGr-7GZJNXg)
<br>
<img src="https://github.com/Ayanabha123456/TraversAppetite/assets/42903837/6d90a008-46a4-4739-95d7-42712e7c4758" width="300">

* [Stove](https://cdn.cloud.scenario.com/assets-transform/efIBNuLjR1exgwKftZnsZA?p=100&Policy=eyJTdGF0ZW1lbnQiOlt7IlJlc291cmNlIjoiaHR0cHM6Ly9jZG4uY2xvdWQuc2NlbmFyaW8uY29tL2Fzc2V0cy10cmFuc2Zvcm0vZWZJQk51TGpSMWV4Z3dLZnRabnNaQT9wPTEwMCoiLCJDb25kaXRpb24iOnsiRGF0ZUxlc3NUaGFuIjp7IkFXUzpFcG9jaFRpbWUiOjE2OTQxMzExOTl9fX1dfQ__&Key-Pair-Id=K36FIAB9LE2OLR&Signature=Iqn~PmOxNwre2kCWduvwtNd7CFevG~l3ehUL9a1Ct4QnSnJm70q0xr2nwbpOaeqPiaB44hZ-GAQesHvRgbFIVzGLQT4--WXd-PqPlQMcmperxOw1nIqyIFtGY9J78oQgtHZJ6LkpaVAQ2dhdOZgxxNDJapEQanCco0JBOPtCT3LgXUwIVt2N8a-FyCdsuP8-oitZQnfpeHbu3ZtiTcWTGL0zuLp49HHZHHGm-aFKygzVTAxkhmCwsvxM0MRiEX6-ckDqC5UdwrawnHAMkY7WGcrzags-ajXPl8mZLme9On~8-~VnokUpMj0X6YR0OftoK61DU2b~Khm3Pyfn1MjB2w__)
<br>
<img src="https://cdn.cloud.scenario.com/assets-transform/efIBNuLjR1exgwKftZnsZA?p=100&Policy=eyJTdGF0ZW1lbnQiOlt7IlJlc291cmNlIjoiaHR0cHM6Ly9jZG4uY2xvdWQuc2NlbmFyaW8uY29tL2Fzc2V0cy10cmFuc2Zvcm0vZWZJQk51TGpSMWV4Z3dLZnRabnNaQT9wPTEwMCoiLCJDb25kaXRpb24iOnsiRGF0ZUxlc3NUaGFuIjp7IkFXUzpFcG9jaFRpbWUiOjE2OTQxMzExOTl9fX1dfQ__&Key-Pair-Id=K36FIAB9LE2OLR&Signature=Iqn~PmOxNwre2kCWduvwtNd7CFevG~l3ehUL9a1Ct4QnSnJm70q0xr2nwbpOaeqPiaB44hZ-GAQesHvRgbFIVzGLQT4--WXd-PqPlQMcmperxOw1nIqyIFtGY9J78oQgtHZJ6LkpaVAQ2dhdOZgxxNDJapEQanCco0JBOPtCT3LgXUwIVt2N8a-FyCdsuP8-oitZQnfpeHbu3ZtiTcWTGL0zuLp49HHZHHGm-aFKygzVTAxkhmCwsvxM0MRiEX6-ckDqC5UdwrawnHAMkY7WGcrzags-ajXPl8mZLme9On~8-~VnokUpMj0X6YR0OftoK61DU2b~Khm3Pyfn1MjB2w__" width="300">

* [Fridge](https://toppng.com/free-image/fridge-PNG-free-PNG-Images_33281)
<br>
<img src="https://toppng.com/uploads/preview/fridge-11533043202z2tuc0ozjk.png" width="300">

* [Cooking Pan](https://cdn.cloud.scenario.com/assets-transform/fuIJ2HewQX2kYigHKr0S9w?p=100&Policy=eyJTdGF0ZW1lbnQiOlt7IlJlc291cmNlIjoiaHR0cHM6Ly9jZG4uY2xvdWQuc2NlbmFyaW8uY29tL2Fzc2V0cy10cmFuc2Zvcm0vZnVJSjJIZXdRWDJrWWlnSEtyMFM5dz9wPTEwMCoiLCJDb25kaXRpb24iOnsiRGF0ZUxlc3NUaGFuIjp7IkFXUzpFcG9jaFRpbWUiOjE2OTQxMzExOTl9fX1dfQ__&Key-Pair-Id=K36FIAB9LE2OLR&Signature=bEeovQ0nIewwCcwO5Tu97z1TS7M1iMVCXAoallzUHJ8uUE2FpZryTctyEGp959e~xrd9EVjEVh9gDIO17bu6pOwGgKuU6SYjQnNFVstBA~QJTY4dG27UxjYoFRJe~MdXWa3-vdJLOvZlElew78q~2reMQL69T~11CjS~GI98UwZw-QfnPnEe1fWlIywO88zhcxUpzrgWXyAw2AUS1UVBDP0UjEMI~nLDuv72w~Jgy7n-XOxmJZ6R-wz4MX17Bhai1LqKtVAtCPt03MyX340IRZR36Wd9YGynwOqNLbH83BzOM6j5jH-rJBpQbVn5F2HninrtMkDCwbQCxQWiHas9lQ__)
<br>
<img src="https://cdn.cloud.scenario.com/assets-transform/fuIJ2HewQX2kYigHKr0S9w?p=100&Policy=eyJTdGF0ZW1lbnQiOlt7IlJlc291cmNlIjoiaHR0cHM6Ly9jZG4uY2xvdWQuc2NlbmFyaW8uY29tL2Fzc2V0cy10cmFuc2Zvcm0vZnVJSjJIZXdRWDJrWWlnSEtyMFM5dz9wPTEwMCoiLCJDb25kaXRpb24iOnsiRGF0ZUxlc3NUaGFuIjp7IkFXUzpFcG9jaFRpbWUiOjE2OTQxMzExOTl9fX1dfQ__&Key-Pair-Id=K36FIAB9LE2OLR&Signature=bEeovQ0nIewwCcwO5Tu97z1TS7M1iMVCXAoallzUHJ8uUE2FpZryTctyEGp959e~xrd9EVjEVh9gDIO17bu6pOwGgKuU6SYjQnNFVstBA~QJTY4dG27UxjYoFRJe~MdXWa3-vdJLOvZlElew78q~2reMQL69T~11CjS~GI98UwZw-QfnPnEe1fWlIywO88zhcxUpzrgWXyAw2AUS1UVBDP0UjEMI~nLDuv72w~Jgy7n-XOxmJZ6R-wz4MX17Bhai1LqKtVAtCPt03MyX340IRZR36Wd9YGynwOqNLbH83BzOM6j5jH-rJBpQbVn5F2HninrtMkDCwbQCxQWiHas9lQ__" width="300">

* [Kitchen background](https://www.vectorstock.com/royalty-free-vector/house-room-with-pink-wall-flat-color-vector-44494775)
<br>
<img src="https://cdn.vectorstock.com/i/1000x1000/47/75/house-room-with-pink-wall-flat-color-vector-44494775.webp" width="300">

* [Logo](https://www.pinclipart.com/maxpin/imbmh/)
<br>
<img src="https://www.pinclipart.com/picdir/big/3-32031_vector-library-stock-chefs-clipart-cooking-demo-chief.png" width="300">

# How to play the game?
* Download the zip file of the game from this [link](https://drive.google.com/file/d/1FZRWKuDbRwjapu3WGFSOawOB5DbG4B2c/view?usp=sharing)
* Unzip the file
* Double click on the *TraversAppetite* .exe file i.e. the application.
* You need to play the game till your character reaches Level 4 to learn all the algorithms. You can continue playing beyond Level 4 but you won't level up.
* The key and mouse mappings are as follows

Action | Control |
:-----------------------: | :-------------------------:
Move Right | <img src="https://www.techonthenet.com/clipart/keyboard/images/letter_d.png" width="100">
Move Left | <img src="https://www.techonthenet.com/clipart/keyboard/images/letter_a.png" width="100">
Interact | <img src="https://www.techonthenet.com/clipart/keyboard/images/letter_e.png" width="100">
UI Interaction, Drag mechanic | <img src="https://icon-library.com/images/left-mouse-button-icon/left-mouse-button-icon-15.jpg" width="100">

See the official [gameplay](https://www.youtube.com/watch?v=EtnMyQ3V4Ok) of TraversAppetite.