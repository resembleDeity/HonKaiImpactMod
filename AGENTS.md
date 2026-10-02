# 本仓库命名与书写规范（强制）

适用范围：本仓库全部代码；其他语言取等效写法。新增与修改代码按此执行，评审按此检查。

## 0. 自检清单（先看这个）

- [ ] 类级（static）变量/字段：`s_`；模块级（global）：`g_`；实例 protected/private：`m_`；public 实例成员：不加前缀
- [ ] private 与 static 同时出现：按 static 规则用 `s_`，不用 `m_`
- [ ] const：一律 `c_`（public / protected / private 都一样；`const` 优先级高于 static 与访问前缀，不写 `s_` / `m_`）。`static readonly` 不是 const，仍按 static 规则用 `s_`
- [ ] bool：在作用域/访问前缀之后追加字母 `b`（无下划线）：`s_bXxx`、`m_bXxx`、`c_bXxx`、`bXxx`、`inbXxx`
- [ ] 参数：输入 `in`、输出 `out`、引用 `ref`
- [ ] class 定义：enum 用前缀 `E`，泛型参数用前缀 `T`，装饰器 class 不加前缀
- [ ] property 不套前缀（裸名，见 §2）；struct（`@dataclass`，或内部字段为类级 static 的数据类）字段不套前缀、不排访问范围（见 §2）
- [ ] class 内顺序：方法/函数在前 → 字段/变量/property 在后；访问权限 public → internal → protected → private；同一权限内实例成员在前、static 成员在后

## 1. 前缀表

| 对象 | 前缀 | 组合写法 | 示例 |
| --- | --- | --- | --- |
| static 变量/字段（类级） | `s_` | `s_` + 名字 | `s_PrefixMap`、`s_StaticProtected` |
| global 变量/字段（模块级） | `g_` | `g_` + 名字 | `g_Modules` |
| bool 变量/字段 | `b` | 紧跟作用域/访问前缀，无下划线 | `s_bRegistered`、`m_bIsDirty`、`bVisible` |
| protected / private 变量/字段（实例） | `m_` | `m_` + 名字 | `m_ProtectedValue`、`m_PrivateValue` |
| private + static 同时出现 | 走 static 规则 | `s_`（不用 `m_`） | `s_bStaticPrivate` |
| const 变量/字段 | `c_` | `c_` + 名字 | `c_MaxRetry`、`c_bEnabled`、`c_DefaultPath` |
| 输入参数 | `in` | `in` + 名字 | `inConfig`、`inLanguageCode` |
| 输出参数 | `out` | `out` + 名字 | `outResult` |
| 引用参数 | `ref` | `ref` + 名字 | `refBuffer` |
| 参数且为 bool | `b` | 参数前缀在前，`b` 紧随 | `inbPara` |
| enum class | `E` | `E` + 名字 | `ENodeType`、`EWorkspaceType` |
| 泛型参数 | `T` | `T` + 名字 | `TType`、`TReturn` |
| 装饰器 class | 无 | — | `ReadonlyProperty`、`StaticReadonlyProperty` |

组合顺序：作用域/访问前缀（`s_` / `m_` / `g_` / `c_`）→ `b` → 名字；参数：`in` / `out` / `ref` → `b` → 名字。

`const` 隐含 static，因此 const 成员的 `c_` 优先于 static 与访问前缀：`private const int c_MaxRetry`（不写 `s_` / `m_`）、`private const bool c_bDebug`。只读但不编译期确定的值（`static readonly`）不算 const，照 static 规则用 `s_`，例如 `s_PrefixMap`。

词形沿用仓库现状（前缀规则不受影响）：class / 方法 / 字段 PascalCase，参数与局部变量 camelCase。

## 2. 豁免对象

| 对象 | 规则 |
| --- | --- |
| **property**（C# 中用 `=>` 定义的那种访问器） | **不适用** static / protected / private 前缀规则，用裸名，也不参与访问范围排序。Python 侧即 `@property`、`ReadonlyProperty`、`StaticReadonlyProperty`：`DisplayName`、`SidebarName`、`IdName`、`PackageName`、`Root`、`Assets`、`Code` |
| **struct**（Python 中是 `@dataclass`，或内部字段为类级 static 的纯数据类） | **不适用访问范围规则**：字段不因 static / protected / private 改名，也不按 public → protected → private 排序，保持字段原名；即使实现上是类级 static（`ClassVar`）也不加 `s_`。如 `NamelessConfig.AddonVersion`、`Versions.Extension`（内部即 static）、`ReportPackage.Tag` |

## 3. class 内书写顺序

1. 优先方法/函数，之后是 字段/变量/property。
2. 访问权限自上而下：public → internal（C# 概念）→ protected → private；同一权限内实例成员在前、static 成员在后。
3. §2 的豁免对象不参与第 2 条的排序：property 与 struct 的字段保持裸名（struct 的方法/函数仍写在字段之前；字段顺序决定 dataclass 的 `__init__` 参数顺序）。

```python
class Example:
	def Function(self, inPara: int, inbPara: bool) -> None: ...  # public
	@classmethod / @staticmethod
	def StaticOrClassMethod(None/cls) -> None: ...  # static
	def PrivateFunction(self, outPara: int) -> None: ...  # private
	@classmethod / @staticmethod
	def PrivateStaticOrClassMethod(None/cls) -> None: ...  # private static

	Value: int  # public
	m_ProtectedValue: int  # protected
	s_StaticProtected: int  # static protected
	m_PrivateValue: int  # private
	s_bStaticPrivate: bool  # static private
```

## 4. 边界说明

- `ReportTag` / `OperatorReturnTag` 是常量容器、不是 `Enum` 子类，因此不套 `E`；真正的 `Enum` 子类（`ENodeTreeType` / `ENodeSocketType` / `ENodeColorTagType` / `ENodeType` / `EWorkspaceType`）才用 `E`。
- struct 可以用 `@dataclass` 声明（如 `NamelessConfig` / `ReportPackage`），也可以是内部字段为类级 static 的类（如 `Versions`）；两种情况都按 §2 豁免前缀与访问范围，仍建议把方法/函数写在字段之前。
- property 的 getter 与普通方法一样使用 `in` / `out` / `ref` 参数前缀。
- **常量容器**（如 `HKIConstants`）：按 §2 的数据类看待，成员不加前缀。若因框架限制必须是编译期常量（例如用作 `AssetMount` 这类 attribute 的实参，`static readonly` 不合法），即使写的是 `const` 也不套 `c_`，保持原名（`HKIConstants.Assets`）。其余能用 `static readonly` 表达的情况照 §1：`const` → `c_`，`static readonly` → `s_`。
- 前缀是 API 的一部分：改名前先查全仓库引用，`README.md` 等文档一并同步。
