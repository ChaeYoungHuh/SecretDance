module DanceGame

open System
open System.Threading
open System.Diagnostics

// ──────────────────────────────────────────
//  유틸리티
// ──────────────────────────────────────────

let clearScreen () = Console.Clear()
let sleep ms = Thread.Sleep(ms : int)

let color c (s: string) =
    Console.ForegroundColor <- c
    printf "%s" s
    Console.ResetColor()

let colorln c (s: string) =
    Console.ForegroundColor <- c
    printfn "%s" s
    Console.ResetColor()

// ──────────────────────────────────────────
//  교수님 아스키 (2프레임)
// ──────────────────────────────────────────

let profFrame1 = """⠀⠀⠀⠀⢀⣄⣠⣶⠶⡶⢿⢛⠻⡛⡍⣍⢍⢍⢍⢛⡛⡛⠿⢷⠶⣶⣀⣄⡀
⠀⣠⣤⢼⢟⣛⠫⠑⠓⠸⡘⢆⢕⢍⠪⡒⢔⠕⢜⠔⡕⢜⢌⢣⠓⡬⠊⠋⠙⢛⢟⢧⣤⣤⣀
⠀⡍⡕⢬⠢⡂⠀⣼⣷⡄⠀⡇⡪⡊⢎⢪⢊⢎⠪⡪⡘⢆⢣⠱⡱⠀⢠⣿⣦⠀⠸⡰⢤⢩⢛⡛⣳⢤⣄⣀
⠀⡪⣘⠢⡣⢅⠀⠻⠿⠃⡀⢎⢼⡟⠛⢻⡧⡻⡛⣾⠛⠛⣻⡱⡘⡀⠘⠿⠟⢀⠸⡰⡑⢎⢢⣕⣱⡮⠿⠃
⠀⠳⠮⣇⣕⡪⢒⠤⡠⡐⡍⡲⢌⡙⣛⢫⠰⣑⠜⣌⢛⡛⣋⢔⢩⢊⠆⡤⢠⢊⣕⣼⠮⠾⠛⠋
⠀⠀⠀⠀⠈⠘⠛⠛⠿⠼⠼⣦⣣⣪⣢⣱⣑⣅⣣⣪⣌⣎⣔⣵⡵⠵⠽⠛⠛⠃
  ⢼ ⡿⣛⣄⠀⠀⠀⣀⣥⣽⣿⣿⣢   ⣪⣿⣿⣿⣤⡁
  ⢟ ⡵⣾⢽⢀⣴⣿⣿⣿⣿⣿⣿⣿⢷⣢ ⣼⡵⣿⣿⣿⣿⣷⣄
  ⣶⣶⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⣿⣿⣿⣿⣿⣿⣿⣿⣷⣄
  ⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣦
⠀⠀⠈⢿⣿⣿⣿⣿⣿⠟⠉⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿ ⢿⣿⣿⣿⣷⣦⣤⣤⡀
⠀⠀⠀⠀⠈⠙⠛⠛⠁⠀⠀⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿ ⠈⠙⠻⣿⣿⢍⡆⠻⣷
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⠿⠻⠟⠟⠿⠻⠟⠿⠻⠟⠿⠿⠻⠀⠀⠀⠈⠉⠒⠽⠿⠧⠝"""

let profFrame2 = """⠀⠀⠀⠀⢀⣄⣠⣶⠶⡶⢿⢛⠻⡛⡍⣍⢍⢍⢍⢛⡛⡛⠿⢷⠶⣶⣀⣄⡀
⠀⣤⢼⢟⣛⠫⠑⠓⠸⡘⢆⢕⢍⠪⡒⢔⠕⢜⠔⡕⢜⢌⢣⠓⡬⠊⠋⠙⢛⢟⢧⣤⣤⣀
⠀⡕⢬⠢⡂⠀⣼⣷⡄⠀⡇⡪⡊⢎⢪⢊⢎⠪⡪⡘⢆⢣⠱⡱⠀⢠⣿⣦⠀⠸⡰⢤⢩⢛⡛⣳⢤⣄⣀
⠀⣘⠢⡣⢅⠀⠻⠿⠃⡀⢎⢼⡟⠛⢻⡧⡻⡛⣾⠛⠛⣻⡱⡘⡀⠘⠿⠟⢀⠸⡰⡑⢎⢢⣕⣱⡮⠿⠃
⠀⠮⣇⣕⡪⢒⠤⡠⡐⡍⡲⢌⡙⣛⢫⠰⣑⠜⣌⢛⡛⣋⢔⢩⢊⠆⡤⢠⢊⣕⣼⠮⠾⠛⠋
⠀⠀⠀⠈⠘⠛⠛⠿⠼⠼⣦⣣⣪⣢⣱⣑⣅⣣⣪⣌⣎⣔⣵⡵⠵⠽⠛⠛⠃
⠀⡿⣛⣄⠀⠀⠀⠀⠀⠀⣀⣥⣽⣿⣿⣢   ⣪⣿⣿⣿⣤⡁
⠀⡵⣾⢽⡅⠀⠀⠀⣴⣿⣿⣿⣿⣿⣿⢷⣢ ⣼⡵⣿⣿⣿⣿⣷⣄
⠀⣧⣶⣿⣿⣿⣴⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⣿⣿⣿⣿⣿⣿⣿⣿⣷⡀
⠀⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⢿⣿⣿⣿⣿⣿⡇⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣦⣀
⠀⠻⣿⣿⣿⣿⣿⣿⣿⠟⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡟⢿⣿⣿⣿⣿⣷⣦⣤⣤⡀
⠀⠀⠀⠈⠙⠛⠛⠁⠀⠀⣸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⠈⠙⠻⢿⣿⣿⢍⡆⠻⣷
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠿⠿⠻⠟⠟⠿⠻⠟⠿⠻⠟⠿⠻⠇⠀ ⠀⠈⠉⠒⠽⠿⠧⠝"""

let profFrame3 = """⠀⠀⠀⠀⠀⢀⣄⣠⣶⠶⡶⢿⢛⠻⡛⡍⣍⢍⢍⢍⢛⡛⡛⠿⢷⠶⣶⣀⣄⡀
⠀⣠⣤⢼⢟⣛⠫⠑⠓⠸⡘⢆⢕⢍⠪⡒⢔⠕⢜⠔⡕⢜⢌⢣⠓⡬⠊⠋⠙⢛⢟⢧⣤⣤⣀
⠀⡍⡕⢬⠢⡂⠀⣼⣷⡄⠀⡇⡪⡊⢎⢪⢊⢎⠪⡪⡘⢆⢣⠱⡱⠀⢠⣿⣦⠀⠸⡰⢤⢩⢛⡛⣳⢤⣄⣀
⠀⡪⣘⠢⡣⢅⠀⠻⠿⠃⡀⢎⢼⡟⠛⢻⡧⡻⡛⣾⠛⠛⣻⡱⡘⡀⠘⠿⠟⢀⠸⡰⡑⢎⢢⣕⣱⡮⠿⠃
⠀⠳⠮⣇⣕⡪⢒⠤⡠⡐⡍⡲⢌⡙⣛⢫⠰⣑⠜⣌⢛⡛⣋⢔⢩⢊⠆⡤⢠⢊⣕⣼⠮⠾⠛⠋
⠀⠀⠀⠀⠈⠘⠛⠛⠿⠼⠼⣦⣣⣪⣢⣱⣑⣅⣣⣪⣌⣎⣔⣵⡵⠵⠽⠛⠛⠃
       ⠀⠀⠀⠀⣀⣥⣽⣿⣿⣢   ⣪⣿⣿⣿⣤⡁
      ⠀⠀⢀⣴⣿⣿⣿⣿⣿⣿⢷⣢ ⣼⡵⣿⣿⣿⣿⣷⣄
⣿⣿⣿⣦  ⣴⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⣿⣿⣿⣿⣿⣿⣿⣿⣷⡀
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⢿⣿⣿⣿⣿⣿⡇⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣦⣀
⠻⣿⣿⣿⣿⣿⣿⣿⣿⠟⠉⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡟⢿⣿⣿⣿⣿⣷⣦⣤⣤⡀
⠀⠀⠈⠙⠛⠛⠛⠛⠁⠀⠀⣸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⠈⠙⠻⢿⣿⣿⢍⡆⠻⣷
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠿⠿⠻⠟⠟⠿⠻⠟⠿⠻⠟⠿⠻⠇⠀⠀⠀⠈⠉⠒⠽⠿⠧⠝"""

let profFrame4 = """⠀⠀⠀⠀⠀⢀⣄⣠⣶⠶⡶⢿⢛⠻⡛⡍⣍⢍⢍⢍⢛⡛⡛⠿⢷⠶⣶⣀⣄⡀
⠀⣠⣤⢼⢟⣛⠫⠑⠓⠸⡘⢆⢕⢍⠪⡒⢔⠕⢜⠔⡕⢜⢌⢣⠓⡬⠊⠋⠙⢛⢟⢧⣤⣤⣀
⠀⡍⡕⢬⠢⡂⠀⣼⣷⡄⠀⡇⡪⡊⢎⢪⢊⢎⠪⡪⡘⢆⢣⠱⡱⠀⢠⣿⣦⠀⠸⡰⢤⢩⢛⡛⣳⢤⣄⣀
⠀⡪⣘⠢⡣⢅⠀⠻⠿⠃⡀⢎⢼⡟⠛⢻⡧⡻⡛⣾⠛⠛⣻⡱⡘⡀⠘⠿⠟⢀⠸⡰⡑⢎⢢⣕⣱⡮⠿⠃
⠀⠳⠮⣇⣕⡪⢒⠤⡠⡐⡍⡲⢌⡙⣛⢫⠰⣑⠜⣌⢛⡛⣋⢔⢩⢊⠆⡤⢠⢊⣕⣼⠮⠾⠛⠋
⠀⠀⠀⠀⠈⠘⠛⠛⠿⠼⠼⣦⣣⣪⣢⣱⣑⣅⣣⣪⣌⣎⣔⣵⡵⠵⠽⠛⠛⠃
       ⠀⠀⠀⠀⣀⣥⣽⣿⣿⣢   ⣪⣿⣿⣿⣤⡁
       ⠀⣴⣿⣿⣿⣿⣿⣿⣿⢷⣢  ⣼⡵⣿⣿⣿⣿⣷
    ⣀⣴⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⣿⣿⣿⣿⣿⣿⣿⣿⡇
 ⣀⣴⣿⣿⣿⣿⣿⣿⢿⣿⣿⣿⣿⣿⣿⣿⡇⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇
⣿⣿⣿⣿⣿⣿⣿⠟⠉ ⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡟⢿⣿⣿⣿⣿⣷⣦⣤⣤⡀
⣿⣿⣿⣿⠟    ⠀⣸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇ ⠻⣿⢿⣿⣿⣿⣿⢍⡆⠻⣷
   ⠀ ⠀⠀⠀⠀⠀⠿⠿⠻⠟⠟⠿⠻⠟⠿⠻⠟⠿⠻⠇⠀⠀⠀  ⠈⠉⠒⠽⠿⠧⠝"""

// 교수님이 뒤돌아볼 때
let profFrameTurn = """
⠀⠀⠀⠀⠀⢀⣀⣀⣤⠤⢦⠴⡜⡝⡝⡝⡝⡧⠦⡴⠤⣤⣀⣀⡀
⢀⡤⢴⢗⢳⢛⢎⢎⢎⢎⢎⢎⢎⢎⢎⢎⢎⢎⢇⢇⢏⢎⢎⢎⢏⢗⢳⠧⡤⣀⣀⡀
⢐⢝⢜⢜⢜⢜⢜⢜⢜⢜⢜⢜⢜⢜⢜⢜⢜⢜⢜⢜⢜⢜⢜⢜⢜⢜⢜⢕⢝⢜⢜⢳⢲⢤⢤⢀
⢐⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢍⣏⣷⠆
⢐⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⢕⣕⢵⢵⠗⠓⠋⠉
⠘⠚⠺⡼⡼⣼⡸⡸⡸⡸⡸⡸⡸⡸⡸⡸⡸⡸⡸⡸⡸⡸⡸⡸⡼⡼⣜⠞⠚⠚⠉⠁
⠀⠀⠀⠀⠀⠈⠉⠉⠹⠚⠞⡾⡽⡽⡽⡽⡽⡽⣝⢷⡗⠛⠉⠉⠁
⠀⠀⠀⠀⠀⠀⠀⠀⠀⡰⡽⡽⡽⡽⡽⡽⣯⡫⡯⡯⣻⡲⡀
⠀⠀⠀⠀⠀⠀⠀⠀⣀⢯⡫⣮⡫⣻⣺⢺⣺⢽⢝⣝⢮⣻⡺⣦⡀
⠀⠀⠀⠀⠀⠀⠀⢠⡺⡵⣝⢮⣟⢮⢮⡳⣽⢝⢷⡹⣕⣷⣝⢮⢧⣂
⠀⠀⠀⠀⠀⠀⠀⡸⡽⣝⢮⣟⢷⢽⢕⣝⢾⢽⢵⢝⣞⢾⣪⢯⡳⣝⢶⢄
⠀⠀⠀⠀⠀⠀⠀⡯⡯⡮⣳⡫⣯⣳⡳⡳⡽⣝⣝⢗⣗⢿⠘⠳⣝⢮⢯⢯⣲⣢⢤⡄"""

// ──────────────────────────────────────────
//  학생 모션 프레임들
// ──────────────────────────────────────────

// 대기 상태
let studentIdle = [
    ""
    "< ･ ･ > < ･ ･ > < ･ ･ >"
    "( ა૮  ) ( ა૮  ) ( ა૮  )"
    ""
]

// 모션1 - 달리기
let motion1A = [
    "    ♪          ♪           ♪"
    "    < ･ ･ >  < ･ ･ >  < ･ ･ >   三"
    "   <（　）>  <（　）>  <（　）>  三"
    "      ＼＼      ＼＼      ＼＼   三"
]
let motion1B = [
    "        ♪          ♪           ♪"
    " 三   < ･ ･ >  < ･ ･ >  < ･ ･ >"
    " 三   <（　）>  <（　）>  <（　）>"
    " 三      / /       / /       / /  "
]

// 모션2 - 흔들기
let motion2A = [
    "    ♪          ♪           ♪"
    " < ･ ･ >     < ･ ･ >     < ･ ･ >"
    " (っﾉJ        (っﾉJ        (っﾉJ"
    "  \ し'        \ し'       \ し'"
]
let motion2B = [
    "    ♪             ♪             ♪"
    "  < ･ ･ >     < ･ ･ >     < ･ ･ >"
    "  \  ⊂        \  ⊂        \  ⊂  "
    "   c し'       c し'       c し'  "
]

// 모션3 - 방방
let motion3A = [
    "  ∩  ♪ ∩     ∩  ♪ ∩    ∩  ♪  ∩"
    " < ゝ ･ >   < ゝ ･ >   < ゝ ･ >"
    " (( /  ﾉ    (( /  ﾉ    (( /  ﾉ"
    "   しーU       しーU       しーU"
]
let motion3B = [
    "  ∩  ♪ ∩     ∩  ♪ ∩    ∩  ♪  ∩"
    " < ゝ ･ >   < ゝ ･ >   < ゝ ･ >"
    "   )  )  ))    )  )  ))    )  )  ))"
    "   Uー|         Uー|         Uー|"
]

// ──────────────────────────────────────────
//  게임 상태
// ──────────────────────────────────────────

type GameState = {
    mutable Score      : int
    mutable Dancing    : bool
    mutable DanceMode  : int       // 0=idle, 1,2,3=모션
    mutable DanceGauge : float     // 0.0 ~ 1.0
    mutable DrainRate  : float     // 초당 감소량
    mutable Running    : bool
    mutable Caught     : bool      // 걸림 여부
    mutable ProfFrame  : int
    mutable DanceFrame : int
    mutable TurnStage  : int       // 0=정면, 1=3프레임, 2=4프레임 전환, 3=뒤돌아본 상태
    mutable StageTimer : float     // 현재 stage 남은 시간
    mutable NextTurnIn : float     // 다음 이벤트까지 남은 시간
    mutable CaughtHold : float     // 뒤돌아본 상태에서 춤 누적 시간
    mutable EndReason  : string    // "", "caught", "classover"
}

// ──────────────────────────────────────────
//  대기 화면
// ──────────────────────────────────────────

let showTitleScreen () =
    let art1 = """
＿/＼／＼   /＼／\     /＼／     //＼／|＿
 ＼　　　　                              　／
 ＜ Dance SECRETLY behind the professor  ＞
 ／ 　　　　　                            \
 ￣|／＼/  ＼/  ＼/    |＼/      ＼/＼/￣
 O
 o
◦⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀                                   z
⠀⠀⠀⠀⢀⣄⣠⣶⠶⡶⢿⢛⠻⡛⡍⣍⢍⢍⢍⢛⡛⡛⠿⢷⠶⣶⣀⣄⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀         z⠀
⠀⣤⢼⢟⣛⠫⠑⠓⠸⡘⢆⢕⢍⠪⡒⢔⠕⢜⠔⡕⢜⢌⢣⠓⡬⠊⠋⠙⢛⢟⢧⣤⣤⣀⠀⠀⠀⠀⠀⠀⠀        
⠀⡕⢬⠢⡂⠀⣼⣷⡄⠀⡇⡪⡊⢎⢪⢊⢎⠪⡪⡘⢆⢣⠱⡱⠀⢠⣿⣦⠀⠸⡰⢤⢩⢛⡛⣳⢤⣄⣀⠀⠀      < = = >⠀< ･ ･ > < ･ ･ > ⠀
⠀⣘⠢⡣⢅⠀⠻⠿⠃⡀⢎⢼⡟⠛⢻⡧⡻⡛⣾⠛⠛⣻⡱⡘⡀⠘⠿⠟⢀⠸⡰⡑⢎⢢⣕⣱⡮⠿⠃⠀⠀      ( ა૮  )⠀( ა૮  ) ( ა૮  ) ⠀
⠀⠮⣇⣕⡪⢒⠤⡠⡐⡍⡲⢌⡙⣛⢫⠰⣑⠜⣌⢛⡛⣋⢔⢩⢊⠆⡤⢠⢊⣕⣼⠮⠾⠛⠋⠀⠀⠀⠀⠀     
⠀⠀⠀⠈⠘⠛⠛⠿⠼⠼⣦⣣⣪⣢⣱⣑⣅⣣⣪⣌⣎⣔⣵⡵⠵⠽⠛⠛⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⡿⣛⣄⠀⠀⠀⠀⠀⠀⣀⣥⣽⣿⣿⣢   ⣪⣿⣿⣿⣤⡁
⠀⡵⣾⢽⡅⠀⠀⠀⣴⣿⣿⣿⣿⣿⣿⢷⣢ ⣼⡵⣿⣿⣿⣿⣷⣄
⠀⣧⣶⣿⣿⣿⣴⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⣿⣿⣿⣿⣿⣿⣿⣿⣷⡀
⠀⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⢿⣿⣿⣿⣿⣿⡇⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣦⣀
⠀⠻⣿⣿⣿⣿⣿⣿⣿⠟⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡟⢿⣿⣿⣿⣿⣷⣦⣤⣤⡀
⠀⠀⠀⠈⠙⠛⠛⠁⠀⠀⣸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⠈⠙⠻⢿⣿⣿⢍⡆⠻⣷
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠿⠿⠻⠟⠟⠿⠻⠟⠿⠻⠟⠿⠻⠇⠀  ⠈⠉⠒⠽⠿⠧⠝
"""
    let art2 = """
＿/＼／＼   /＼／\     /＼／     //＼／|＿
 ＼　　　　                              　／
 ＜ Dance SECRETLY behind the professor  ＞
 ／ 　　　　　                            \
 ￣|／＼/  ＼/  ＼/    |＼/      ＼/＼/￣
 O
 o
◦⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀                                   z
⠀⠀⠀⠀⠀⢀⣄⣠⣶⠶⡶⢿⢛⠻⡛⡍⣍⢍⢍⢍⢛⡛⡛⠿⢷⠶⣶⣀⣄⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀       z
⠀⣠⣤⢼⢟⣛⠫⠑⠓⠸⡘⢆⢕⢍⠪⡒⢔⠕⢜⠔⡕⢜⢌⢣⠓⡬⠊⠋⠙⢛⢟⢧⣤⣤⣀⠀⠀⠀⠀⠀⠀        
⠀⡍⡕⢬⠢⡂⠀⣼⣷⡄⠀⡇⡪⡊⢎⢪⢊⢎⠪⡪⡘⢆⢣⠱⡱⠀⢠⣿⣦⠀⠸⡰⢤⢩⢛⡛⣳⢤⣄⣀⠀⠀     < _ _ > < ･ ･ > < ･ ･ > ⠀
⠀⡪⣘⠢⡣⢅⠀⠻⠿⠃⡀⢎⢼⡟⠛⢻⡧⡻⡛⣾⠛⠛⣻⡱⡘⡀⠘⠿⠟⢀⠸⡰⡑⢎⢢⣕⣱⡮⠿⠃⠀⠀     ( ა૮  ) ( ა૮  ) ( ა૮  ) ⠀
⠀⠳⠮⣇⣕⡪⢒⠤⡠⡐⡍⡲⢌⡙⣛⢫⠰⣑⠜⣌⢛⡛⣋⢔⢩⢊⠆⡤⢠⢊⣕⣼⠮⠾⠛⠋⠀⠀⠀⠀⠀⠀    
⠀⠀⠀⠀⠈⠘⠛⠛⠿⠼⠼⣦⣣⣪⣢⣱⣑⣅⣣⣪⣌⣎⣔⣵⡵⠵⠽⠛⠛⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
  ⢼ ⡿⣛⣄⠀⠀⠀⣀⣥⣽⣿⣿⣿⣢   ⣪⣿⣿⣿⣤⡁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
  ⢟ ⡵⣾⢽⢀⣴⣿⣿⣿⣿⣿⣿⣿⢷⣢ ⣼⡵⣿⣿⣿⣿⣷⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
  ⣶⣶⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⣿⣿⣿⣿⣿⣿⣿⣿⣷⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
  ⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣦⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠈⢿⣿⣿⣿⣿⣿⠟⠉⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿ ⢿⣿⣿⣿⣷⣦⣤⣤⡀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠈⠙⠛⠛⠁⠀⠀⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿ ⠈⠙⠻⣿⣿⢍⡆⠻⣷⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀ ⠀⢸⠿⠻⠟⠟⠿⠻⠟⠿⠻⠟⠿⠿⠻⠀⠀ ⠈⠉⠒⠽⠿⠧⠝⠀
"""




    let frames = [| art1; art2 |]
    let mutable i = 0
    while not Console.KeyAvailable do
        clearScreen ()
        colorln ConsoleColor.DarkYellow frames[i]
        printfn ""
        colorln ConsoleColor.DarkGray "  [ A ] Dance / next motion    [ D ] Stop"
        colorln ConsoleColor.DarkGray "  Press any key to start!"
        sleep 500
        i <- (i + 1) % 2
    Console.ReadKey(true) |> ignore

// ──────────────────────────────────────────
//  게이지 렌더링
// ──────────────────────────────────────────

let renderGauge (danceGauge: float) (drainRate: float) (score: int) =
    let width = 40
    let filledCount = int (danceGauge * float width)
    let bar =
        String.init width (fun i ->
            if i < filledCount then "█" else "░"
        )
    let gaugePercent = int (danceGauge * 100.0)
    printfn ""
    color ConsoleColor.DarkYellow "  Dance gauge: "
    let gaugeColor =
        if danceGauge < 0.2 then ConsoleColor.Red
        elif danceGauge < 0.5 then ConsoleColor.Yellow
        else ConsoleColor.Green
    colorln gaugeColor (sprintf "[%s] %d%%" bar gaugePercent)
    color ConsoleColor.DarkGray (sprintf "  Drain rate: %.3f/s  " drainRate)
    colorln ConsoleColor.Cyan (sprintf "Score: %d" score)

// ──────────────────────────────────────────
//  학생 프레임 출력
// ──────────────────────────────────────────

let renderStudents (state: GameState) =
    let lines =
        if not state.Dancing then
            studentIdle
        else
            let frame = state.DanceFrame % 2
            match state.DanceMode with
            | 1 -> if frame = 0 then motion1A else motion1B
            | 2 -> if frame = 0 then motion2A else motion2B
            | 3 -> if frame = 0 then motion3A else motion3B
            | _ -> studentIdle
    for line in lines do
        colorln ConsoleColor.Cyan (sprintf "  %s" line)

// ──────────────────────────────────────────
//  교수님 렌더링
// ──────────────────────────────────────────

let renderProf (state: GameState) =
    let art =
        match state.TurnStage with
        | 1 -> profFrame3
        | 2 -> profFrame4
        | 3 -> profFrameTurn
        | _ ->
            if state.ProfFrame % 2 = 0 then profFrame1
            else profFrame2
    let profColor =
        match state.TurnStage with
        | 1 | 2 -> ConsoleColor.Magenta
        | 3 -> ConsoleColor.Red
        | _ -> ConsoleColor.White
    colorln profColor art

// ──────────────────────────────────────────
//  전체 화면 렌더링
// ──────────────────────────────────────────

let renderScreen (state: GameState) =
    clearScreen ()
    renderStudents state
    printfn ""
    renderProf state
    renderGauge state.DanceGauge state.DrainRate state.Score

// ──────────────────────────────────────────
//  종료 화면
// ──────────────────────────────────────────

let showGameOver (score: int) (endReason: string) =
    clearScreen ()
    if endReason = "caught" then
        colorln ConsoleColor.Red "  ████ CAUGHT! ████"
        printfn ""
        colorln ConsoleColor.White """　   　 <      >
　   　 ┏( 　)┛
　    　　θｰＪ"""
        printfn ""
        colorln ConsoleColor.White """　   <      >　   　 <      >
　   ┏( 　)┛　   　   ┏( 　)┛
　     θｰＪ 　　        θｰＪ"""
        printfn ""
        colorln ConsoleColor.White """<      >
┏( 　)┛
   θｰＪ"""
        printfn ""
        colorln ConsoleColor.Yellow (sprintf "  Final score: %d" score)
    elif endReason = "classover" then
        printfn ""
        colorln ConsoleColor.Yellow "  The class is over.."
        printfn ""
        colorln ConsoleColor.White """　   　 <˘▿ ˘>⠀< ^▿ ^ > <'▿ ' >
　   　 ( ა૮  ) ( ა૮  ) ( ა૮  )   ~ ~ ~ """
        printfn ""
        colorln ConsoleColor.Cyan (sprintf "  Final score: %d" score)
    else
        colorln ConsoleColor.DarkGray "  Game over"
        printfn ""
        colorln ConsoleColor.Cyan (sprintf "  Final score: %d" score)
    printfn ""
    colorln ConsoleColor.DarkGray "  Press any key to exit..."
    Console.ReadKey(true) |> ignore

// ──────────────────────────────────────────
//  메인 게임 루프
// ──────────────────────────────────────────

[<EntryPoint>]
let main _ =
    Console.OutputEncoding <- Text.Encoding.UTF8
    Console.CursorVisible  <- false
    let rng = Random()

    showTitleScreen ()

    let state = {
        Score      = 0
        Dancing    = false
        DanceMode  = 0
        DanceGauge = 1.0
        DrainRate  = 0.062
        Running    = true
        Caught     = false
        ProfFrame  = 0
        DanceFrame = 0
        TurnStage  = 0
        StageTimer = 0.0
        NextTurnIn = 1.25
        CaughtHold = 0.0
        EndReason  = ""
    }

    // ±a few frames of timing jitter (~60fps)
    let timingJitter () = float (rng.Next(-4, 5)) * (1.0 / 60.0)

    let withJitter (seconds: float) = max 0.05 (seconds + timingJitter ())

    // gentle early game, ramps up later (quadratic on elapsed, slope slightly eased)
    let scheduleNextTurn (elapsed: float) =
        let baseWait = max 0.08 (1.35 - elapsed * 0.012 - elapsed * elapsed * 0.0042)
        withJitter (baseWait + rng.NextDouble() * 0.40)

    let turnStageDuration (elapsed: float) =
        let baseDur = max 0.05 (0.34 - elapsed * 0.0035 - elapsed * elapsed * 0.0011)
        withJitter baseDur

    let turnedHoldDuration (elapsed: float) =
        let baseDur = max 0.50 (1.85 - elapsed * 0.016 - elapsed * elapsed * 0.0052)
        withJitter baseDur

    // brief front-facing pause before / after a turn (2–6 frames)
    let preTurnHesitate () = float (rng.Next(2, 7)) / 60.0
    let postTurnHesitate () = float (rng.Next(2, 6)) / 60.0

    // continuous dance while prof is fully turned → caught
    let caughtDanceLimit (elapsed: float) =
        max 0.15 (0.38 - elapsed * 0.005 - elapsed * elapsed * 0.0015)

    // 스페이스바 토글: 누를 때마다 모션1→2→3→멈춤→1 순환
    let mutable dancePressCount = 0

    // 게이지 감소 가속 타이머
    let speedTimer = Stopwatch.StartNew()
    let mutable lastSpeedUp = 0.0

    // 렌더/업데이트 타이머
    let renderTimer = Stopwatch.StartNew()
    let mutable lastTick = 0.0
    let mutable lastRender = 0.0
    let mutable lastDanceFrame = 0.0
    let mutable lastProfFrame = 0.0

    // 점수 타이머
    let scoreTimer = Stopwatch.StartNew()
    let mutable lastScore = 0.0

    // 키 입력 전용 스레드
    // A = 춤 시작/모션 변경
    // D = 춤 멈춤
    let inputThread = Thread(fun () ->
        while state.Running do
            let k = Console.ReadKey(true)
            match k.Key with
            | ConsoleKey.A ->
                // 춤 시작 or 다음 모션 (idle이든 춤추는 중이든 A는 "다음 모션")
                dancePressCount <- dancePressCount + 1
                let mode = ((dancePressCount - 1) % 3) + 1   // 1,2,3 순환
                state.Dancing <- true
                state.DanceMode <- mode
                state.DanceFrame <- 0
                lastScore <- scoreTimer.Elapsed.TotalSeconds
            | ConsoleKey.D when state.Dancing ->
                // 즉시 멈춤
                state.Dancing <- false
                state.DanceMode <- 0
                lastScore <- scoreTimer.Elapsed.TotalSeconds
            | _ -> ()
    )
    inputThread.IsBackground <- true
    inputThread.Start()

    // 시작 전 키 버퍼 비우기 (타이틀 화면 키 잔류 제거)
    while Console.KeyAvailable do Console.ReadKey(true) |> ignore

    while state.Running do
        let now = renderTimer.Elapsed.TotalSeconds
        let dt = now - lastTick
        let safeDt = if dt > 0.0 then dt else 0.0
        lastTick <- now

        // ── 점수 업데이트 (춤추는 중 0.01초마다 1점) ──
        let profFacingFront =
            state.TurnStage = 0 || state.TurnStage = -1 || state.TurnStage = 4
        if state.Dancing && profFacingFront then
            let scoreDelta = scoreTimer.Elapsed.TotalSeconds - lastScore
            if scoreDelta >= 0.01 then
                state.Score <- state.Score + int (scoreDelta / 0.01)
                lastScore <- scoreTimer.Elapsed.TotalSeconds

        // ── 춤 게이지 처리 (춤추면 회복, 멈추면 감소) ──
        if safeDt > 0.0 then
            if state.Dancing then
                state.DanceGauge <- min 1.0 (state.DanceGauge + 0.10 * safeDt)
            else
                state.DanceGauge <- max 0.0 (state.DanceGauge - state.DrainRate * safeDt)
            if state.DanceGauge <= 0.0 then
                state.EndReason <- "classover"
                state.Running <- false

        // ── 게이지 감소 속도 가속 (매 5초마다) ──
        let elapsed = speedTimer.Elapsed.TotalSeconds
        if elapsed - lastSpeedUp >= 5.0 then
            lastSpeedUp <- elapsed
            state.DrainRate <- min 0.28 (state.DrainRate * 1.18)

        // ── 교수님 뒤돌아보기 이벤트 (3 -> 4 -> turned) ──
        if safeDt > 0.0 then
            let elapsedNow = renderTimer.Elapsed.TotalSeconds
            match state.TurnStage with
            | 0 ->
                state.NextTurnIn <- state.NextTurnIn - safeDt
                if state.NextTurnIn <= 0.0 then
                    state.TurnStage <- -1
                    state.StageTimer <- preTurnHesitate ()
            | -1 ->
                state.StageTimer <- state.StageTimer - safeDt
                if state.StageTimer <= 0.0 then
                    state.TurnStage <- 1
                    state.StageTimer <- turnStageDuration elapsedNow
            | 1 ->
                state.StageTimer <- state.StageTimer - safeDt
                if state.StageTimer <= 0.0 then
                    state.TurnStage <- 2
                    state.StageTimer <- turnStageDuration elapsedNow
            | 2 ->
                state.StageTimer <- state.StageTimer - safeDt
                if state.StageTimer <= 0.0 then
                    state.TurnStage <- 3
                    state.StageTimer <- turnedHoldDuration elapsedNow
                    state.CaughtHold <- 0.0
            | 3 ->
                state.StageTimer <- state.StageTimer - safeDt
                if state.Dancing then
                    state.CaughtHold <- state.CaughtHold + safeDt
                else
                    state.CaughtHold <- 0.0

                if state.CaughtHold >= caughtDanceLimit elapsedNow then
                    state.Caught <- true
                    state.EndReason <- "caught"
                    state.Running <- false
                elif state.StageTimer <= 0.0 then
                    state.TurnStage <- 4
                    state.StageTimer <- postTurnHesitate ()
                    state.CaughtHold <- 0.0
            | 4 ->
                state.StageTimer <- state.StageTimer - safeDt
                if state.StageTimer <= 0.0 then
                    state.TurnStage <- 0
                    state.NextTurnIn <- scheduleNextTurn renderTimer.Elapsed.TotalSeconds
            | _ -> ()

        // ── 댄스 프레임 (0.2초마다) ──
        if now - lastDanceFrame >= 0.2 then
            state.DanceFrame <- state.DanceFrame + 1
            lastDanceFrame <- now

        // ── 교수님 정면 프레임 (0.5초마다) ──
        if now - lastProfFrame >= 0.5 then
            state.ProfFrame <- state.ProfFrame + 1
            lastProfFrame <- now

        // ── 화면 렌더 (0.1초마다) ──
        if now - lastRender >= 0.1 then
            renderScreen state
            lastRender <- now

        Thread.Sleep(16)  // ~60fps tick

    Console.CursorVisible <- true
    if state.EndReason = "" then
        state.EndReason <- if state.Caught then "caught" else "classover"
    showGameOver state.Score state.EndReason
    0