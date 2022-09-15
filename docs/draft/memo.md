# メモ

## global class diagram

```mermaid
classDiagram

    %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
    %% Token
    class Token {
        TokenType Type
        string Text

        int GetValue()
    }
    class NumberToken {
        
    }
    class OperatorToken {
        OperatorKind Kind
        Token? LeftHand
        Token? RightHand
    }

    Token<|--NumberToken
    Token<|--OperatorToken

    %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
    %% Tokenizer
    class TokenizerUtils {
        <<static>>
    }
    class Tokenizer {

    }
    TokenizerUtils ..> Tokenizer:use

    class ParseTreeUtils {
        <<static>>
    }
    class ParseTree {

    }
    ParseTreeUtils ..> ParseTree:use

```


## Tokenizer

### 状態一覧

| 状態 | 概要                    |
| ---- | ----------------------- |
| [SO] | 初期状態                |
| Num0 | 数字`0`後区切り待ち状態 |
| Num1 | 数字`0`後区切り待ち状態 |
| Op   | 演算子後区切り待ち状態  |

### 状態遷移図

```mermaid
stateDiagram-v2
    
    [S0]-->Num0 : 0
    [S0]-->Num1 : 1--9

    Num0-->[S0] : WhiteSpace, EOL
    Num0-->Op  : Operator

    Num1-->[S0] :  WhiteSpace, EOL
    Num1-->Op :   Operator
    Num1-->Num1 : 0--9

    [S0] --> Op : Operator
    Op --> Op : Operator
    Op --> Num0 :  0
    Op --> Num1 :  1--9
    Op --> [S0] : WhiteSpace, EOL

    [S0]-->[S0]: WhiteSpace, EOL
```

### 状態による処理対応表

| 状態     | 0                  | 1--9               | Operator           | WhiteSpace | EOL     |
| -------- | ------------------ | ------------------ | ------------------ | ---------- | ------- |
| **S0**   | `NewToken`         | `NewToken`         | `NewToken`         | -          | -       |
| **Num0** | *error*            | *error*            | `DrainAndNewToken` | `Drain`    | `Drain` |
| **Num1** | `KeepCharacter`    | `KeepCharacter`    | `DrainAndNewToken` | `Drain`    | `Drain` |
| **Op**   | `DrainAndNewToken` | `DrainAndNewToken` | `DrainAndNewToken` | `Drain`    | `Drain` |

## Parser

### 状態遷移図
```mermaid
stateDiagram-v2
    S0-->S1 : NumberToken
    S0-->S2 : OperatorToken(IsPulusMinus)

    S1-->S0 : OperatorToken

    S2-->S1 : NumberToken


```

### 状態遷移表

| 状態 | NumberToken  | OperatorToken(IsPlusMinus) | OperatorToken(Other) |
| ---- | ------------ | -------------------------- | -------------------- |
| S0   | AppendToTree | SetSign                    | Error                |
| S1   | Error        | SetOperator                | SetOperator          |
| S2   | AppendToTree | Error                      | Error                |

