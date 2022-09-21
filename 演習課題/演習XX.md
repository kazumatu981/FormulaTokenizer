# 演習XX(魔改造の夜)

> 以下の課題は、世の中で全く役に立たない思考ゲーム。
> 設計力向上には、このような課題を日夜チャレンジしてリファクタリングを繰り返す事で自然と身につく。
> 
> さぁ、ゲームの始まりだ

## 魔改造1: `()`を使えるようにして演算の優先度をつけよ。

## 魔改造2: 変数が使えるようにせよ。

### 魔改造2-1: 計算機クラスを設計する。
```csharp
var calc = new Calc();

var result = calc.Run("3*3 + 4*4");

Assert.Equal(25, result);
```

### 魔改造2-2: 「変数ストレージ」を設計して、定数はその「ストレージ」の初期とせよ。

```csharp
var storage = new Storage();

storage.Add("x", 3);
storage.Add("y", 4);

Assert.Equal(3, storage.Get("x"));
Assert.Equal(4, storage.Get("y"));
```

### 魔改造2-3: 定数を使えるようにせよ。

```csharp
var storage = new Storage();

storage.Add("x", 3);
storage.Add("y", 4);

var calc = new Calc(storage);

var result = calc.Run("x*x + y*y");

Assert.Equal(25, result);
```

### 魔改造2-4: 代入式を解釈できるようにする。

```csharp
var calc = new Calc();

_ = calc.Run("x=3");
_ = calc.Run("y=4");
var result = calc.Run("x*x + y*y");

Assert.Equal(25, result);
```

> このまま魔改造を続けると、ファイルに記述された計算手順にしたがって、計算を続ける「オレオレスクリプト」が完成する。

## 魔改造3：有理数環上に拡張せよ

```csharp
var rationalCalc = new RationalCalc();

var result = rationalCalc.Run(" 1 / 2  + 1/3");

Assert.Equal(5, result.p);
Assert.Equal(6, result.q);
```

> `RationalNumber`という名前のクラスを設計して。。。。。
> 素数判定のメソッドを用意して、素因数分解して。。。。。
> 意外と大変です。

## 魔改造4：複素平面（[ガウス整数環](https://ja.wikipedia.org/wiki/%E3%82%AC%E3%82%A6%E3%82%B9%E6%95%B4%E6%95%B0)）上に拡張せよ

> 整数を係数にもつ複素数は「和」、「積」について演算として閉じている。
> さらに「和」は逆元を持っているが、「積」は逆元を持っていない。
> これは、整数と同じ構造を持っていることから、**ガウス整数環**と呼ばれている。

```csharp
var gausianCalc = new GaussianIntegerCalc();

var result = gausianCalc.Run("(1+i) * (1-i)");

Assert.Type<Gausian>(resuslt);
Assert.Equal(1, result.Real);
Assert.Equal(1, result.Imaginary);
```
