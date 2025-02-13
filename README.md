# ASFChatDumper

![ASFChatDumper](https://socialify.git.ci/chr233/ASFChatDumper/image?description=1&forks=1&language=1&name=1&owner=1&pattern=Diagonal%20Stripes&stargazers=1&theme=Auto)

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/62490e8e5bbd4a038374246409b24f12)](https://app.codacy.com/gh/chr233/ASFChatDumper/dashboard)
![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/chr233/ASFChatDumper/publish.yml?logo=github)
[![License](https://img.shields.io/github/license/chr233/ASFChatDumper?logo=apache)](https://github.com/chr233/ASFChatDumper/blob/master/license)

[![GitHub Release](https://img.shields.io/github/v/release/chr233/ASFChatDumper?logo=github)](https://github.com/chr233/ASFChatDumper/releases)
[![GitHub Release](https://img.shields.io/github/v/release/chr233/ASFChatDumper?include_prereleases&label=pre-release&logo=github)](https://github.com/chr233/ASFChatDumper/releases)
![GitHub last commit](https://img.shields.io/github/last-commit/chr233/ASFChatDumper?logo=github)

![GitHub Repo stars](https://img.shields.io/github/stars/chr233/ASFChatDumper?logo=github)
[![GitHub Download](https://img.shields.io/github/downloads/chr233/ASFChatDumper/total?logo=github)](https://img.shields.io/github/v/release/chr233/ASFChatDumper)

[![Bilibili](https://img.shields.io/badge/bilibili-Chr__-00A2D8.svg?logo=bilibili)](https://space.bilibili.com/5805394)
[![Steam](https://img.shields.io/badge/steam-Chr__-1B2838.svg?logo=steam)](https://steamcommunity.com/id/Chr_)

[![Steam](https://img.shields.io/badge/steam-donate-1B2838.svg?logo=steam)](https://steamcommunity.com/tradeoffer/new/?partner=221260487&token=xgqMgL-i)
[![爱发电][afdian_img]][afdian_link]
[![buy me a coffee][bmac_img]][bmac_link]

【ASF】Steam聊天记录导出工具: [https://keylol.com/t989418-1-1](https://keylol.com/t989418-1-1)

## 安装方式

### 初次安装 / 手动更新

1. 从 [GitHub Releases](https://github.com/chr233/ASFChatDumper/releases) 下载插件的最新版本
2. 解压后将 `ASFChatDumper.dll` 丢进 `ArchiSteamFarm` 目录下的 `plugins` 文件夹
3. 重新启动 `ArchiSteamFarm` , 使用命令 `ASFChatDumper` 或 `ACD` 来检查插件是否正常工作

### 捐赠

|               ![img][afdian_qr]                |                   ![img][bmac_qr]                   |                       ![img][usdt_qr]                       |
| :--------------------------------------------: | :-------------------------------------------------: | :---------------------------------------------------------: |
| ![爱发电][afdian_img] <br> [链接][afdian_link] | ![buy me a coffee][bmac_img] <br> [链接][bmac_link] | ![USDT][usdt_img] <br> `TW41eecZ199QK6zujgKP4j1cz2bXzRus3c` |

[afdian_qr]: https://raw.chrxw.com/chr233/master/afadian_qr.png
[afdian_img]: https://img.shields.io/badge/爱发电-@chr__-ea4aaa.svg?logo=github-sponsors
[afdian_link]: https://afdian.com/@chr233
[bmac_qr]: https://raw.chrxw.com/chr233/master/bmc_qr.png
[bmac_img]: https://img.shields.io/badge/buy%20me%20a%20coffee-@chr233-yellow?logo=buymeacoffee
[bmac_link]: https://www.buymeacoffee.com/chr233
[usdt_qr]: https://raw.chrxw.com/chr233/master/usdt_qr.png
[usdt_img]: https://img.shields.io/badge/USDT-TRC20-2354e6.svg?logo=bitcoin

## 插件配置说明

> 本插件的配置不是必须的, 保持默认配置即可使用大部分功能

### ASF.json

```json
{
  //ASF 配置
  "CurrentCulture": "...",
  "IPCPassword": "...",
  "...": "...",
  //ASFChatDumper 配置
  "ASFChatDumper": {
    "EULA": true,
    "Statistic": true
  }
}
```

| 配置项      | 类型   | 默认值 | 说明                                                               |
| ----------- | ------ | ------ | ------------------------------------------------------------------ |
| `EULA`      | `bool` | `true` | 是否同意 `EULA`                                                    |
| `Statistic` | `bool` | `true` | 是否允许发送统计数据, 仅用于统计插件用户数量, 不会发送任何其他信息 |

## 插件指令说明

| 命令                 | 缩写  | 权限            | 说明                                                                       |
| -------------------- | ----- | --------------- | -------------------------------------------------------------------------- |
| `ASFChatDumper`      | `ACD` | `FamilySharing` | 查看 ASFChatDumper 的版本                                                  |
| `DumpChat [Bots]`    | `DC`  | `Master`        | 导出指定机器人的聊天记录到 `plugin/chat_dump` 中, 不同人的聊天记录分开保存 |
| `DumpChatMix [Bots]` | `DCM` | `Master`        | 导出指定机器人的聊天记录到 `plugin/chat_dump` 中, 聊天记录不分开保存       |

---

[![Repobeats analytics image](https://repobeats.axiom.co/api/embed/df6309642cc2a447195c816473e7e54e8ae849f9.svg "Repobeats analytics image")](https://github.com/chr233/ASFChatDumper/pulse)

---

[![Star History Chart](https://api.star-history.com/svg?repos=chr233/ASFChatDumper&type=Date)](https://star-history.com/#chr233/ASFChatDumper&Date)

---
