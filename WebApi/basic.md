## ASP.NET-WebAPI2

### 什么是 `ASP.NET-WebAPI`？

`ASP.NET Web API` 是一种框架，用于轻松构建可以访问多种客户端（包括浏览器和移动设备）的 `HTTP` 服务。`ASP.NET Web API` 是一种用于在 `.NET Framework` 上构建`RESTful` 应用程序的理想平台。

### 关于`RESTful`的理解

【1】`REST` 是英文 `representational state transfer`(表象性状态转变)或者表述性状态转移; `Rest` 是 `web` 服务的一种架构风格; 使用 `HTTP`,`URI`,`XML`,`JSON`,`HTML`

等广泛流行的标准和协议;轻量级,跨平台,跨语言的架构设计; 它是一种设计风格,不是一种标准,是一种思想。

【2】`Rest` 架构的主要原则：

-- 事务为对象，资源有统一，形式有多样（`xml`、`json` 等）

-- 每个资源都有一个唯一的资源标识符。

-- 同一个资源具有多种表现形式(`xml`,`json` 等)。

-- 对资源的各种操作不会改变资源标识符。

-- 所有的操作都是无状态的。

【3】什么是 `RESTful`？

符合 `REST` 原则的架构方式即可称为 `RESTful`。

【4】为什么会出现 restful？

在 Restful 之前我们写一个 web 请求，格式通常是这样的：

-- http://192.168.1.100/product/query/1 根据用户 id 查询商品

-- http://192.168.1.100/product/Add 新增商品

-- http://192.168.1.100/product/update 修改商品

-- http://192.168.1.100/product/delete 删除商品

Restful 用法：

-- http://192.168.1.100/product/1 GET 根据用户 id 查询商品

-- http://192.168.1.100/product/ POST 新增商品

-- http://192.168.1.100/product/ PUT 修改商品

-- http://192.168.1.100/product/ DELETE 删除商品

restful 风格其实就是根据请求的类型（get、post、put、delete）来匹配对应的方法。
