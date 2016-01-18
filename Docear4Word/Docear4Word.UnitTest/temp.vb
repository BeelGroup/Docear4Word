'% Bibtex4word - A collection of macros for inserting bibtex references
'%               into a Microsoft word document
'%
'% See http://www.ee.ic.ac.uk/hp/staff/dmb/perl/index.html for installation and ussage notes
'%
'%      Copyright (C) Mike Brookes 2006-2011
Const Version = "$Id: Bibtex4Word.vba,v 1.92 2011/07/21 11:11:23 dmb Exp $"
'%
'%   Home page: http://www.ee.ic.ac.uk/hp/staff/dmb/
'%
'%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
'%   This program is free software; you can redistribute it and/or modify
'%   it under the terms of the GNU General Public License as published by
'%   the Free Software Foundation; either version 2 of the License, or
'%   (at your option) any later version.
'%
'%   This program is distributed in the hope that it will be useful,
'%   but WITHOUT ANY WARRANTY; without even the implied warranty of
'%   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'%   GNU General Public License for more details.
'%
'%   You can obtain a copy of the GNU General Public License from
'%   ftp://prep.ai.mit.edu/pub/gnu/COPYING-2.0 or by writing to
'%   Free Software Foundation, Inc.,675 Mass Ave, Cambridge, MA 02139, USA.
'%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

#If VBA7 Then
Private Declare PtrSafe Function OpenProcess Lib "kernel32" (ByVal dwDesiredAccess As Long, ByVal bInheritHandle As Long, ByVal dwProcessID As Long) As Long
Private Declare PtrSafe Function CloseHandle Lib "kernel32" (ByVal hObject As Long) As Long
Private Declare PtrSafe Function WaitForSingleObject Lib "kernel32" (ByVal hHandle As Long, ByVal dwMilliseconds As Long) As Long
Declare PtrSafe Function CreateProcess Lib "kernel32" _
                                   Alias "CreateProcessA" (ByVal _
      lpApplicationName As String, ByVal lpCommandLine As String, ByVal _
      lpProcessAttributes As Long, ByVal lpThreadAttributes As Long, _
      ByVal bInheritHandles As Long, ByVal dwCreationFlags As Long, _
      ByVal lpEnvironment As Long, ByVal lpCurrentDirectory As String, _
      lpStartupInfo As STARTUPINFO, lpProcessInformation As _
      PROCESS_INFORMATION) As LongPtr
Private Declare PtrSafe Function GetExitCodeProcess Lib "kernel32" (ByVal hProcess As Long, lpExitCode As Long) As Long
Private Declare PtrSafe Function MultiByteToWideChar Lib "kernel32" ( _
    ByVal CodePage As Long, ByVal dwFlags As Long, _
    ByVal lpMultiByteStr As LongPtr, ByVal cchMultiByte As Long, _
    ByVal lpWideCharStr As LongPtr, ByVal cchWideChar As Long) As Long
#Else
Private Declare Function OpenProcess Lib "kernel32" (ByVal dwDesiredAccess As Long, ByVal bInheritHandle As Long, ByVal dwProcessID As Long) As Long
Private Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Long) As Long
Private Declare Function WaitForSingleObject Lib "kernel32" (ByVal hHandle As Long, ByVal dwMilliseconds As Long) As Long
Private Declare Function CreateProcessA Lib "kernel32" (ByVal _
      lpApplicationName As String, ByVal lpCommandLine As String, ByVal _
      lpProcessAttributes As Long, ByVal lpThreadAttributes As Long, _
      ByVal bInheritHandles As Long, ByVal dwCreationFlags As Long, _
      ByVal lpEnvironment As Long, ByVal lpCurrentDirectory As String, _
      lpStartupInfo As STARTUPINFO, lpProcessInformation As _
      PROCESS_INFORMATION) As Long
Private Declare Function GetExitCodeProcess Lib "kernel32" (ByVal hProcess As Long, lpExitCode As Long) As Long
Private Declare Function MultiByteToWideChar Lib "kernel32" ( _
    ByVal CodePage As Long, ByVal dwFlags As Long, _
    ByVal lpMultiByteStr As Long, ByVal cchMultiByte As Long, _
    ByVal lpWideCharStr As Long, ByVal cchWideChar As Long) As Long
#End If
Const CitePrefix As String = "BIB_"
Const NoCitePrefix As String = "B4B_"
Const BibBookmark As String = "BIB__bib"
Const MagicDefault As String = ":/"         ' Default magic chars for search and flags
Const DoneGetStyle As Integer = 1
Const UndoZOpt As Integer = 1               ' allow undo operations
Private Const NORMAL_PRIORITY_CLASS = &H20&
Private Const INFINITE = -1&
Private Const SW_SHOWMINIMIZED = 2&         ' from winuser.h
Private Const SW_NORMAL = 1&         ' from winuser.h
Private Const SW_HIDE = 0&                  ' from winuser.h
Private Const STARTF_USESHOWWINDOW = 1&                  ' from ???.h

' STARTUPINFO structure: see http://msdn2.microsoft.com/en-us/library/ms686331.aspx
#If VBA7 Then
Private Type STARTUPINFO
      cb As Long
      lpReserved As String
      lpDesktop As String
      lpTitle As String
      dwX As Long
      dwY As Long
      dwXSize As Long
      dwYSize As Long
      dwXCountChars As Long
      dwYCountChars As Long
      dwFillAttribute As Long
      dwFlags As Long
      wShowWindow As Integer
      cbReserved2 As Integer
      lpReserved2 As Long
      hStdInput As LongPtr
      hStdOutput As LongPtr
      hStdError As LongPtr
End Type
#Else
Private Type STARTUPINFO
      cb As Long
      lpReserved As String
      lpDesktop As String
      lpTitle As String
      dwX As Long
      dwY As Long
      dwXSize As Long
      dwYSize As Long
      dwXCountChars As Long
      dwYCountChars As Long
      dwFillAttribute As Long
      dwFlags As Long
      wShowWindow As Integer
      cbReserved2 As Integer
      lpReserved2 As Long
      hStdInput As Long
      hStdOutput As Long
      hStdError As Long
End Type
#End If

Private Type PROCESS_INFORMATION
      hProcess As Long
      hThread As Long
      dwProcessID As Long
      dwThreadID As Long
End Type

Dim BibStyle As String                      ' Current style
Dim StyleFlags As String                    ' Current style flags
Dim MagicChars As String                    ' Magic characters [default = ":/"]
Dim FileHead As String                      ' temp filenames without extensions
Dim CiteList As String                      ' list of cited references in document (beginning and ending with a comma
Dim BibkeyOrder As String                   ' ordered list of encoded bibkeys
Dim CiteCount As Integer                    ' number of citations
Dim BibFile As String                       ' BibTex Data file (but without .bib extension)
Dim DoneAction As Integer                   ' avoid repeating actions
Dim CiteField As Field                      ' field used when searching citations
Dim CiteRefs As String                      ' string containing a string of refs in the form " xaaa ybbb zccc "
Dim CiteRange As Range                      ' range of detected citation string including [] delimiters
Dim UpdDisp As Boolean                      ' true if cite entries show bibliography labels (normal state)
Dim Comma As String                         ' String to place between references

Public Function ExecCmd(cmdline) As Long
    Dim proc As PROCESS_INFORMATION
    Dim start As STARTUPINFO

    ' Initialize the STARTUPINFO structure:
    start.cb = Len(start)
    start.wShowWindow = SW_HIDE      ' see http://msdn2.microsoft.com/en-us/library/ms633548.aspx
    start.dwFlags = STARTF_USESHOWWINDOW

    ' Start the shelled application: see http://msdn2.microsoft.com/en-us/library/ms682425.aspx
#If VBA7 Then
    Call CreateProcess(vbNullString, cmdline, 0&, 0&, 1&, NORMAL_PRIORITY_CLASS, 0&, vbNullString, start, proc)
#Else
    Call CreateProcessA(vbNullString, cmdline, 0&, 0&, 1&, NORMAL_PRIORITY_CLASS, 0&, vbNullString, start, proc)
#End If
    ' Wait for the shelled application to finish:
    ret& = WaitForSingleObject(proc.hProcess, INFINITE)
    Call GetExitCodeProcess(proc.hProcess, ret&)
    Call CloseHandle(proc.hThread)
    Call CloseHandle(proc.hProcess)
    ExecCmd = ret&
End Function
Public Function BinToUni(bySrc() As Byte, cp As Long) As String
' Converts a UTF-8 byte array to a Unicode string
Dim lBytes As Long, lNC As Long, lRet As Long


    lBytes = UBound(bySrc) - LBound(bySrc) + 1
    lNC = lBytes
    BinToUni = String$(lNC, Chr(0))
    lRet = MultiByteToWideChar(cp, 0, VarPtr(bySrc(LBound(bySrc))), lBytes, StrPtr(BinToUni), lNC)
    BinToUni = Left$(BinToUni, lRet)
    BinToUni = Replace(BinToUni, vbCr & vbLf, vbLf)
    BinToUni = Replace(BinToUni, vbCr, vbLf)
    ' possibly should also convert Unicode &H85, &HC, &H2028 and &H2029 for completeness
End Function
Sub bibtex4word_settips()
    '
    ' Run this routine once only to set the tooltip entries on the toolbar
    '
    CommandBars("bibtex4word").Controls("TemplateProject.Bibtex4Word.bibtex4word_disp").TooltipText = "Toggle Citation Display"
    CommandBars("bibtex4word").Controls("TemplateProject.Bibtex4Word.bibtex4word_style").TooltipText = "Define Bibtex Style"
    CommandBars("bibtex4word").Controls("TemplateProject.Bibtex4Word.bibtex4word_add").TooltipText = "Insert Citation"
    CommandBars("bibtex4word").Controls("TemplateProject.Bibtex4Word.bibtex4word_bib").TooltipText = "Insert/Update Bibliography"
    CommandBars("bibtex4word").Controls("TemplateProject.Bibtex4Word.bibtex4word_save").TooltipText = "Create Local Bibtex File"
    CommandBars("bibtex4word").Controls("TemplateProject.Bibtex4Word.bibtex4word_files").TooltipText = "Define Bibtex File"
End Sub
Function encodekey(rawkey As String) As String
    ' encode a bibtex key so that it uses only alphanumerics
    encodekey = Replace(LCase(rawkey), "_", "_5f")
    i = 1
    While i <= Len(encodekey)
        ch = Mid(encodekey, i, 1)
        If ch < "0" Or ch > "z" Or (ch > "9" And ch < "a" And ch <> "_") Then
            encodekey = Replace(encodekey, ch, "_" & LCase(Hex(Asc(ch))))
            i = i + 3
        Else
        i = i + 1
        End If
    Wend
End Function
Function decodekey(enckey As String) As String
    ' decode a key back to its true value (including non-alphanumerics)
    decodekey = Replace(LCase(enckey), "_", vbLf)   ' replace all "_" with something that never occurs
    i = InStr(decodekey, vbLf)
    While i > 0
        av = 0
        vch = True
        For j = 1 To 2
            cv = Asc(Mid(decodekey, i + j, 1)) - 48
            If cv < 0 Then vch = False
            If cv > 9 Then
                cv = cv - 39
                If cv < 10 Or cv > 15 Then vch = False
            End If
            av = av * 16 + cv
        Next j
        If av < 33 Then vch = False
        If vch Then
            decodekey = Replace(decodekey, Mid(decodekey, i, 3), Chr(av))
        Else
            decodekey = Replace(decodekey, Mid(decodekey, i, 3), "_" & Mid(decodekey, i + 1, 2))            ' revert to original
        End If
        i = InStr(decodekey, vbLf)
    Wend
End Function
Function StyleNum(i As Variant, line As String, Optional zval As Long = 0, Optional ival As Long = 1) As Long
' extract a numerical prefix from the style flags line
    xline = "x" & line                  ' prefix by x so that while loop ends
    mult = 1
    StyleNum = 0
    If i = 0 Then
        StyleNum = zval                 ' zval [0] is the default value for when i=0 (means style flag is missing)
    Else
        Do
            ch = Mid(xline, i, 1)
            If ch >= "0" And ch <= "9" Then
                StyleNum = StyleNum + mult * (Asc(ch) - 48)
                mult = mult * 10
                i = i - 1
            Else
                Exit Do
            End If
        Loop
        If mult = 1 Then StyleNum = ival ' ival [1] is the default value for when no number is specified
    End If
End Function
Function MatchBrace(i As Variant, line As String) As Integer
' find matching brace at the current level
' *** this function should really take account of escaped braces e.g. \}
' it might be faster to use builtin find and replace functions
    MatchBrace = 0
    lev = 0
    For j = i To Len(line)
        If Mid(line, j, 1) = "}" Then
            If lev = 0 Then
                MatchBrace = j
                Exit For
            Else
                lev = lev - 1
            End If
        ElseIf Mid(line, j, 1) = "{" Then
            lev = lev + 1
        End If
    Next j
End Function
Function SortCite(InList As String) As String
' InList has the form " xalpha ybeta " where x,y lie in [,/-nx] and alpha, beta are encoded keys
' codes: "/"=nocite, "n"=centre of a list, "-"=start of a list, ","=end list or isolated, "x" = other [wrong]
' note that there is a space at both ends
' output is sorted and compressed in bibliography order but with / entries first
' Outputs
'       CiteRefs    global string containing a string of refs in the form " xaaa ybbb zccc "
'                   where x,y,z are in "/n-," and aaa,bbb,ccc are encoded bibtex keys


    CiteList = InList                                       ' local copy of input
    
    'MsgBox "Unsorted: " & CiteList
    
    ' if not sorting, it would be more efficient just to move the nocites to the front
    
    lencite = Len(CiteList)
    If lencite > 1 Then
        If InStr(StyleFlags, "s") Then dodosort = True         ' /s = sort, else just move nocites to the front

        i = 1
        k = InStrRev(CiteList, " ", lencite - 1)                ' find final citation
        'If i < lencite Then Mid(CiteList, k + 2, 1) = ","            ' force final cite to have , [need to do this earlier]
        bki = Mid(CiteList, i + 2, InStr(i + 2, CiteList, " ") - i - 2) ' extract encoded bibkey
        If dodosort Then opi = InStr(BibkeyOrder, " " & bki & " ") Else opi = 0
        If Mid(CiteList, i + 1, 1) = "/" Then opi = opi - 1000000     ' / represents nocite and we put these first
        Do While i < k
            j = i
            jn = InStr(j + 1, CiteList, " ")
            opj = opi
            changed = False
            Do While j < k
                jp = j                                  ' see if we should swap j and j-next
                opjp = opj
                j = jn
                jn = InStr(j + 1, CiteList, " ")
                bkj = Mid(CiteList, j + 2, InStr(j + 2, CiteList, " ") - j - 2)
                If dodosort Then opj = InStr(BibkeyOrder, " " & bkj & " ") Else opj = 0
                If Mid(CiteList, j + 1, 1) = "/" Then opj = opj - 1000000     ' / represents nocite and we put these first
                If opj < opjp Then
                    If jp = i Then opi = opj            ' update opi if we are replacing head
                    If j = k Then k = jp + jn - j       ' update tail marker if replacing tail
                    Mid(CiteList, jp, jn - jp) = Mid(CiteList, j, jn - j) & Mid(CiteList, jp, j - jp)
                    opj = opjp
                    j = jp + jn - j
                    changed = True
                End If
            Loop
            k = InStrRev(CiteList, " ", k - 1)
            If Not changed Then Exit Do
        Loop
        
        'MsgBox "Sorted: " & CiteList
    
        i = 1
        Do While i < lencite
            If Mid(CiteList, i + 1, 1) <> "/" Then Exit Do Else i = InStr(i + 1, CiteList, " ")
        Loop
        If InStr(StyleFlags, "c") Then            ' /c = compress
        
            bki = Mid(CiteList, i + 2, InStr(i + 2, CiteList, " ") - i - 2) ' extract encoded bibkey
            opi = InStr(BibkeyOrder, " " & bki & " ")
            If opi = 0 Then
                koi = -10
            Else
                koi = Len(Replace(BibkeyOrder, " ", "", opi)) + opi  ' number of keys after this one in bibliography
            End If
            
            k = InStrRev(CiteList, " ", lencite - 1)          ' last entry
            seqlen = 0                                          ' length of sequence
            Do While i < k
                j = InStr(i + 1, CiteList, " ")
                bkj = Mid(CiteList, j + 2, InStr(j + 2, CiteList, " ") - j - 2)
                opj = InStr(BibkeyOrder, " " & bkj & " ")
                If opj = 0 Then
                    koj = -10
                Else
                    koj = Len(Replace(BibkeyOrder, " ", "", opj)) + opj  ' number of keys after this one in bibliography
                End If
                If koj = koi + 1 Then
                    If seqlen = 0 Then Mid(CiteList, i + 1, 1) = "-" Else Mid(CiteList, i + 1, 1) = "n"
                    seqlen = seqlen + 1
                Else
                    If seqlen = 1 Then Mid(CiteList, ip + 1, 1) = ","   ' abloish previously started compression
                    Mid(CiteList, i + 1, 1) = ","
                    seqlen = 0
                End If
                ip = i
                i = j
                koi = koj
            Loop
            If seqlen = 1 Then Mid(CiteList, ip + 1, 1) = ","   ' correct a short sequence
            
            'MsgBox "Compressed: " & CiteList
        Else
        Do While i < lencite
            Mid(CiteList, i + 1, 1) = ","
            i = InStr(i + 1, CiteList, " ")
        Loop
        
            'MsgBox "Sorted: " & CiteList

        End If
        
    End If
    SortCite = CiteList
End Function
Sub NextCiteList(doup As Integer)
    ' get the next string of citations in the document or selected paragraph
    ' Inputs
    '       CiteField   global that points to the field to start searching from
    '       doup        =1 if we should update the fields as we pass them
    ' Outputs
    '       CiteRefs    global string containing a string of refs in the form " xaaa ybbb zccc "
    '                   where x,y,z are either / or , and aaa,bbb,ccc are encoded bibtex keys
    '       CiteField   points to the next available field
    
        'Set CiteField = ActiveDocument.Fields(1)               ' example iitialization
    Dim DelRange As Range
    Dim bibtext As String
    CiteRefs = " "
    nociting = True
    Do While Not (CiteField Is Nothing)
        myPos = InStr(1, CiteField.Code, CitePrefix)                          ' chech for citation
        citepos = myPos
        If myPos = 0 Then myPos = InStr(1, CiteField.Code, NoCitePrefix)      ' or hidden citation
        If (CiteField.Type = wdFieldRef) And (myPos > 0) Then
        
            ' extract the bibtex key eliminating any formatting options
            
            done = True                                            ' default is to quit
            bibtext = Mid(CiteField.Code, myPos + 4)
            myPos = InStr(1, bibtext, " ")
            If myPos > 0 Then bibtext = Mid(bibtext, 1, myPos - 1)
            myPos = InStr(1, bibtext, "\")
            If myPos > 0 Then bibtext = Mid(bibtext, 1, myPos - 1)
            
            ' include the starting "[" if it exists
            
            If CiteRefs = " " Then                                  ' for first citation in the string
                Set CiteRange = CiteField.Code
                CiteRange.start = CiteRange.start - 1
                If CiteRange.start > 0 Then
                    CiteRange.End = CiteRange.start
                    CiteRange.start = CiteRange.start - 1
                    If InStr("[(", CiteRange.Text) = 0 Then CiteRange.start = CiteRange.End
                End If
            End If
            
            If citepos > 0 Then nociting = False
            If nociting Then chdelim = "/" Else chdelim = ","
            CiteRefs = CiteRefs & chdelim & bibtext & " "
            If doup > 0 And ActiveDocument.Bookmarks.Exists(CitePrefix & bibtext) Then
                If UpdDisp Then
                    CiteField.Update
                Else
                ' we want to just replace the text with the key name but unfortunately this
                ' fails if the existing text is of zero length as for the no-cite and compressed entries. So
                ' in these cases, we change the REF to the cite bookmark, then update the text and then
                ' change it back again. Ugly but it works.
                    If citepos = 0 Then
                        codetext = CiteField.Code.Text
                        CiteField.Code.Text = Replace(codetext, NoCitePrefix, CitePrefix, 1, 1)
                        CiteField.Update
                        If nociting Then
                            CiteField.Result.Text = "</" & decodekey(bibtext) & ">"
                        Else
                            CiteField.Result.Text = "<" & decodekey(bibtext) & ">"
                        End If
                        CiteField.Code.Text = codetext
                    Else
                        CiteField.Result.Text = "<" & decodekey(bibtext) & ">"
                    End If
                End If
            End If

            ' now check to see if the next field follows immediately
            
            Set DelRange = CiteField.Result                           ' result range
            If DelRange.start < DelRange.End Then DelRange.start = DelRange.End + 1
            CiteRange.End = DelRange.start                            ' end of this field
            DelRange.End = DelRange.start + 1
            If InStr(")]", DelRange.Text) > 0 Then CiteRange.End = DelRange.End                          ' closing "]" character
            Set CiteField = CiteField.Next                          ' set to next field before messing around with this one
            If Not (CiteField Is Nothing) Then                  ' if last field in document then it ends a list
                If (CiteField.Type = wdFieldRef) Then          ' any non-REF field ends the list
                    If InStr(1, CiteField.Code, CitePrefix) + InStr(1, CiteField.Code, NoCitePrefix) > 0 Then
                        nfstart = CiteField.Code.start
                        DelRange.End = nfstart - 1
                        If DelRange.start < DelRange.End Then               ' adjacent fields never end the string
                            done = Not (Trim(DelRange.Text) Like "[,;-]")       ' must have a valid delimiter char
                        Else
                            done = False
                        End If
                    End If
                End If
            End If
            If done Then Exit Do
        Else
            Set CiteField = CiteField.Next                          ' set to next field
        End If
    Loop
End Sub
Private Sub WriteCiteList()
'   write out a citation list to follow existing selection and then delete the selection
'   Inputs after call to sortcite:
'       CiteRefs    global string containing a string of refs in the form " xaaa ybbb zccc "
'                   where x,y,z are in "/n-," and aaa,bbb,ccc are encoded bibtex keys
'                   prefix codes: "/"=nocite, "n"=centre of a list, "-"=start of a list, ","=end list or isolated,

    Dim BibkeyEnc As String
    Dim PrevFont As Font
'   First we put an "=" before and after the selection because otherwise it will not delete
'   properly if it consists entirely of null-length fields
'   However, we leave the selection in place in order to preserve its formatting for the new list
    Selection.InsertAfter "="
    Selection.InsertBefore "="

    Set prevrange = Selection.Range
    Selection.Collapse Direction:=wdCollapseEnd
    Selection.MoveStart Unit:=wdCharacter, Count:=-1
    Set PrevFont = Selection.Font.Duplicate ' remember previous font settings of the "="
    Selection.Collapse Direction:=wdCollapseEnd
    CiteRefs = SortCite(CiteRefs)
    lencite = Len(CiteRefs)
    If lencite > 1 Then                         ' don't insert anything if there is nothing to insert
        If InStr(StyleFlags, "(") Then bracket = "(" Else bracket = "["
        Selection.InsertAfter bracket
        Set BracketRange = Selection.Range     ' to use for non preexisting
        Selection.Collapse Direction:=wdCollapseEnd
        prefix = ""
        i = 1
        Do While i < lencite                     ' delete other nocite marks
            If Len(prefix) > 0 Then
                Selection.InsertAfter prefix
                Selection.Collapse Direction:=wdCollapseEnd
            End If
            BibkeyEnc = Mid(CiteRefs, i + 2, InStr(i + 2, CiteRefs, " ") - i - 2)
            prefix = Mid(CiteRefs, i + 1, 1)
            bkmktext = CitePrefix & BibkeyEnc   ' we need to guarantee that this bookmark has real text
            preexist = ActiveDocument.Bookmarks.Exists(bkmktext)
            If Not preexist And InStr(BibkeyEnc, "_") > 0 Then      ' for compatibility with pre V1.12
                BibkeyEnc = encodekey(decodekey(BibkeyEnc))         ' eliminate invalid encodings
                bkmktext = CitePrefix & BibkeyEnc
                preexist = ActiveDocument.Bookmarks.Exists(bkmktext)
            End If
            If Not preexist Then
                ActiveDocument.Bookmarks.Add Name:=CitePrefix & BibkeyEnc, Range:=BracketRange
                ActiveDocument.Bookmarks.Add Name:=NoCitePrefix & BibkeyEnc, Range:=BracketRange
            End If
            If InStr(StyleFlags, "h") Then bkmktext = bkmktext & " \h"  ' Make the citation clickable
            If prefix Like "[/n]" And (UpdDisp Or Not preexist) Then
                bkmktext = NoCitePrefix & BibkeyEnc
            End If
            Set newcite = ActiveDocument.Fields.Add(Range:=Selection.Range, Type:=wdFieldRef, Text:=bkmktext, PreserveFormatting:=True)
            newcite.Update                                          ' must update even if we will overwrite the text
            If Not (UpdDisp And preexist) Then
                If prefix = "/" Then
                    newcite.Result.Text = "</" & decodekey(BibkeyEnc) & ">"
                Else
                    newcite.Result.Text = "<" & decodekey(BibkeyEnc) & ">"
                End If
            End If
            If prefix Like "[/n]" Then
                prefix = ""
                If Not UpdDisp And preexist Then
                newcite.Code.Text = Replace(newcite.Code.Text, CitePrefix, NoCitePrefix, 1, 1)
                End If
            Else
                If prefix = "," Then prefix = Comma  ' convert comma to the chosen separator
            End If
            i = InStr(i + 1, CiteRefs, " ")
            Selection.Collapse Direction:=wdCollapseEnd
        Loop
        stringend = Selection.End
        If bracket = "(" And Not prefix = "" Then
            Selection.InsertAfter ")"
            Selection.start = BracketRange.start
        ElseIf InStr(StyleFlags, "[") Or prefix = "" Then
            Selection.start = BracketRange.start             ' delete initial bracket
            Selection.End = BracketRange.End
            Selection.Delete
            Selection.End = stringend - 1
        Else
            Selection.InsertAfter "]"
            Selection.start = BracketRange.start
        End If
        If InStr(StyleFlags, "^") Then Selection.Font.Superscript = True
        Selection.Font.Hidden = False
        ' now update font characteristics except small caps, superscript
        Selection.Font.Size = PrevFont.Size
        Selection.Font.Name = PrevFont.Name
        Selection.Font.Bold = PrevFont.Bold
        Selection.Font.Italic = PrevFont.Italic
        Selection.Font.Borders = PrevFont.Borders
        Selection.Font.Color = PrevFont.Color
        Selection.Font.Emboss = PrevFont.Emboss
        Selection.Font.EmphasisMark = PrevFont.EmphasisMark
        Selection.Font.Engrave = PrevFont.Engrave
        Selection.Font.Outline = PrevFont.Outline
        Selection.Font.Scaling = PrevFont.Scaling
        Selection.Font.Shadow = PrevFont.Shadow
        Selection.Font.Underline = PrevFont.Underline
        Selection.Font.UnderlineColor = PrevFont.UnderlineColor
        Selection.Collapse Direction:=wdCollapseEnd
    End If
    prevrange.Delete
End Sub
Sub bibtex4word_add()
    Const searchmax = 10
    Dim searchterm(1 To searchmax) As String
    Dim bibkey As String
    
    smartcut = Options.SmartCutPaste
    Options.SmartCutPaste = False
    DoneAction = 0                      ' reset action flags
    getstyle                            ' make sure the magic chars are set
    If (Selection.Type = wdSelectionIP) Or (Selection.Type = wdSelectionNormal) Then
        BibFile = GetBibFile & ".bib"
        If Dir(BibFile) = "" Then BibFile = ""
        bibfileopen = False
        bibfilewarn = False

' First we check to see if the selection includes any part of an existing citation
' If so, we edit that one rather than inserting a new one
' in this case, we extract the existing list of keys as bibkeylist

' Note: field selection values depend on whether or not the field result (r chars) is empty
'   (1) r=0: myfield.select = (a,b), myfield.code = (a+1,b-1), myfield.result = (b,b)
'   (2) r>0: myfield.select = (a,b), myfield.code = (a+1,b-r-2), myfield.result = (b-r-1,b-1)
' It is best to regard start and end values as pointing to the gaps between characters
' even if start=end, selection.text gives the next character


        SelStart = Selection.start
        SelEnd = Selection.End
        Selection.Expand Unit:=wdParagraph
        CiteRefs = " "                                      ' in case no fields to search
        If Selection.Fields.Count >= 1 Then
            Set CiteField = Selection.Fields(1)
            Do While Not (CiteField Is Nothing)
                NextCiteList (0)
                If SelStart <= CiteRange.End And SelEnd >= CiteRange.start Then Exit Do Else CiteRefs = " "
            Loop
        End If

' If we are editing an existing list, then expand selection to include brackets [ ] at each end

        If CiteRefs = " " Then
            Selection.start = SelStart
            Selection.End = SelEnd
            Selection.Collapse Direction:=wdCollapseEnd
        Else
            Selection.start = CiteRange.start
            Selection.End = CiteRange.End
        End If
        
' now extract a list of decoded keys

        bibkeys = ""
        i = 1
        lencite = Len(CiteRefs)
        Do While i < lencite
            j = InStr(i + 1, CiteRefs, " ")
            If Mid(CiteRefs, i + 1, 1) = "/" Then
                bibkeys = bibkeys & ",/" & decodekey(Mid(CiteRefs, i + 2, j - i - 2))
            Else
                bibkeys = bibkeys & "," & decodekey(Mid(CiteRefs, i + 2, j - i - 2))
            End If
            i = j
        Loop
        bibkeylist = Mid(bibkeys, 2)           ' remove initial comma
        CiteRefs = " "                      ' reset
        
' ideally we should give a list of keys in the bibtex database like Lyx does
        
        verpos = InStr(Version, ",v")
        vernum = Mid(Version, verpos + 3, InStr(verpos + 3, Version, " ") - verpos - 2)
        bibkeys = LCase(Trim(InputBox("Enter comma-separated bibtex keys or search using "": word1 word2 A:author T:title B:booktitle J:journal K:keywords""", "BibTex4Word V" & vernum & ": Citation Entry", bibkeylist)))
        bibkeylist = ","
        Do While bibkeys <> ""
            CommaPos = InStr(bibkeys, ",")
            If CommaPos > 0 Then
                bibkey = Trim(Mid(bibkeys, 1, CommaPos - 1))
                bibkeys = Trim(Mid(bibkeys, CommaPos + 1, Len(bibkeys) - CommaPos))
            Else
                bibkey = bibkeys
                bibkeys = ""
            End If
            If Left(bibkey, 1) = "/" Then
                citecode = "/"
                bibkey = Trim(Mid(bibkey, 2))
            Else
                citecode = ","
            End If
            If bibkey <> "" Then

' If the key includes ":" then it is actually a search string

                magicsearch = Mid(MagicChars, 1, 1)
                If InStr(bibkey, magicsearch) > 0 Then
                    If citecode = "/" Then Uncite = "/" Else Uncite = ""
                    searchkeys = bibkey
                    numterms = 0
                    oldkeys = 0
                    searchtype = " "
                    searchtypes = ""
                    Do While searchkeys <> ""
                        Spacepos = InStr(searchkeys, " ")
                        If Spacepos > 0 Then
                            searchkey = Mid(searchkeys, 1, Spacepos - 1)
                            searchkeys = Trim(Mid(searchkeys, Spacepos + 1, Len(searchkeys) - Spacepos))
                        Else
                            searchkey = searchkeys
                            searchkeys = ""
                        End If
                        Select Case InStr(searchkey, magicsearch)
                            Case 1
                                searchtype = " "
                                searchkey = Mid(searchkey, 2)
                            Case 2
                                searchtype = LCase(Mid(searchkey, 1, 1))
                                searchkey = Mid(searchkey, 3)
                        End Select
                        If searchkey <> "" Then
                            numterms = numterms + 1
                            searchterm(numterms) = searchkey
                            searchtypes = searchtypes & searchtype
                        End If
                    Loop
                    If numterms > 0 Then
                        If BibFile = "" Then
                            If Not bibfilewarn Then
                                MsgBox ("Cannot open database file")
                                bibfilewarn = True
                            End If
                        Else
                            If Not bibfileopen Then
                                Open BibFile For Input As #6
                                bibfileopen = True
                            Else
                                Seek #6, 1
                            End If
                            matchcount = 0
                            matchlist = ""
                            thiskey = ""
                            termsremain = numterms  ' prevent a match first time around
                            Do While Not EOF(6)    ' Loop until end of file
                                Line Input #6, textlinein    ' Read line into variable.
                                textlinein = LCase(textlinein)
                                lineinlen = Len(textlinein)
                                linestart = 1
                                Do While linestart <= lineinlen
                                    lfpos = InStr(linestart, textlinein, vbLf)
                                    If lfpos = 0 Then lfpos = Len(textlinein) + 1
                                    textline = Mid(textlinein, linestart, lfpos - linestart)
                                    If Left(textline, 1) = "@" Then
                                        ' check if previous entry satisfied conditions
                                        If termsremain = 0 Then
                                            If InStr(bibkeylist, "," & thiskey & ",") = 0 Then
                                                matchlist = matchlist & Uncite & thiskey & ", "
                                                matchcount = matchcount + 1
                                            Else
                                                oldkeys = oldkeys + 1
                                            End If
                                        End If
                                        ' reset search conditions for next entry
                                        termsremain = numterms
                                        termslist = searchtypes
                                        fieldchar = "$"
                                        pos = InStrRev(textline, "{")
                                        If pos = 0 Or Left(textline, 8) = "@comment" Then
                                            thiskey = ""        ' ignore @comment entries
                                        Else
                                            thiskey = Mid(textline, pos + 1, Len(textline) - pos - 1)
                                        End If
                                    Else
                                        If thiskey <> "" And termsremain > 0 Then
                                            pos = InStr(textline, "{")
                                            posq = InStr(textline, """")
                                            If posq > 0 And (pos = 0 Or posq < pos) Then pos = poq
                                            poseq = InStr(textline, "=")
                                            If poseq > 0 And poseq < pos Then
                                                fieldname = Trim(Mid(textline, 1, poseq - 1))
                                                Select Case fieldname
                                                    Case "author", "title", "keywords", "journal", "booktitle"
                                                        fieldchar = Mid(fieldname, 1, 1)
                                                    Case Else
                                                        fieldchar = "$"
                                                End Select
                                            Else
                                                pos = 1     ' continuation line - search from column 1
                                            End If
                                            ' check all fields for some keywords
                                            termnum = InStr(termslist, " ")
                                            Do While termnum > 0
                                                If InStr(pos, textline, searchterm(termnum)) > 0 Then
                                                    termsremain = termsremain - 1
                                                    Mid(termslist, termnum, 1) = "="
                                                End If
                                                termnum = InStr(termnum + 1, termslist, " ")
                                            Loop
                                            If fieldchar <> "$" Then
                                                termnum = InStr(termslist, fieldchar)
                                                Do While termnum > 0
                                                    If InStr(pos, textline, searchterm(termnum)) > 0 Then
                                                        termsremain = termsremain - 1
                                                        Mid(termslist, termnum, 1) = "="
                                                    End If
                                                    termnum = InStr(termnum + 1, termslist, fieldchar)
                                                Loop
                                            End If
                                        End If
                                    End If
                                    linestart = lfpos + 1
                                Loop
                            Loop
                            ' check to see if the last entry met the conditions
                            If termsremain = 0 Then
                                matchlist = matchlist & Uncite & thiskey & ", "
                                matchcount = matchcount + 1
                            End If
                            ' now we have a list of matches
                            Select Case matchcount
                                Case 0
                                    If oldkeys = 0 Then MsgBox ("no matches found for " & bibkey)
                                Case 1
                                    bibkeys = matchlist & bibkeys
                                Case 2, 3, 4, 5
                                    matchlist = LCase(Trim(InputBox(matchcount & " matches found - please select", "BibTex Search Results", Mid(matchlist, 1, Len(matchlist) - 2))))
                                    If matchlist <> "" Then bibkeys = matchlist & ", " & bibkeys
                                Case Else
                                    If MsgBox("Do you want all " & matchcount & " matches found for " & bibkey, vbYesNo) = vbYes Then bibkeys = matchlist & bibkeys
                            End Select
                        End If
                    Else
                        MsgBox ("No valid search terms in " & bibkey)
                    End If

' Not a search string, just a normal key

                Else
                    If InStr(bibkeylist, "," & bibkey & ",") = 0 Then
                        bibkeylist = bibkeylist & bibkey & ","
                        CiteRefs = CiteRefs & citecode & encodekey(bibkey) & " "
                    End If
                End If
            End If
        Loop
        
        If CiteRefs <> " " Then
            UpdDisp = True
            For Each prop In ActiveDocument.CustomDocumentProperties
                If prop.Name = "BIBDISP" Then
                    If LCase(Left(prop.Value, 1)) = "k" Then UpdDisp = False        ' k means display keys
                    Exit For
                End If
            Next
            If InStr(StyleFlags, ";") > 0 Then Comma = ";" Else Comma = ","
            If InStr(StyleFlags, ",") = 0 Then Comma = Comma & " "
            'If Selection.start < Selection.End Then Selection.Delete
            WriteCiteList                           ' sort and write
            Selection.Collapse Direction:=wdCollapseEnd
        End If
    Else
        MsgBox "Place cursor where you want the crossreference"
        Application.ScreenUpdating = True
        Error 1
    End If
    Close
    Options.SmartCutPaste = smartcut
    Application.ScreenUpdating = True
End Sub
Sub bibtex4word_disp()
    Dim bibtext As String
    
    smartcut = Options.SmartCutPaste
    Options.SmartCutPaste = False
    DoneAction = 0                      ' reset action flags
    foundprop = False
    bibdisp = "key"
    For Each prop In ActiveDocument.CustomDocumentProperties
        If prop.Name = "BIBDISP" Then
            If LCase(Left(prop.Value, 1)) = "k" Then bibdisp = "ref"
            foundprop = True
            Exit For
        End If
    Next
    If foundprop Then
        prop.Value = bibdisp
    Else
        ActiveDocument.CustomDocumentProperties.Add Name:="BIBDISP", LinkToContent:=False, Value:=bibdisp, Type:=msoPropertyTypeString
    End If
    UpdDisp = Left(bibdisp, 1) <> "k"
    For i = 1 To 3                          ' 1=Document, 2=Footnotes, 3=Endnotes
        Select Case i
            Case 1
                nnotes = 1
            Case 2
                nnotes = ActiveDocument.Footnotes.Count
            Case 3
                nnotes = ActiveDocument.Endnotes.Count
        End Select
        If nnotes > 0 Then
            For j = 1 To nnotes
                Select Case i
                    Case 1
                        nfields = ActiveDocument.Fields.Count
                        If nfields > 0 Then
                            Set CiteField = ActiveDocument.Fields(1)
                        End If
                    Case 2
                        nfields = ActiveDocument.Footnotes(j).Range.Fields.Count
                        If nfields > 0 Then
                            Set CiteField = ActiveDocument.Footnotes(j).Range.Fields(1)
                        End If
                    Case 3
                        nfields = ActiveDocument.Endnotes(j).Range.Fields.Count
                        If nfields > 0 Then
                            Set CiteField = ActiveDocument.Endnotes(j).Range.Fields(1)
                        End If
                End Select
                If nfields >= 1 Then
                    While Not (CiteField Is Nothing)
                        NextCiteList (1)
                    Wend
                End If
            Next j
        End If
    Next i
    Options.SmartCutPaste = smartcut
    Application.ScreenUpdating = True
End Sub
Private Sub bibrefresh()
    Application.ScreenUpdating = False
    UpdDisp = True
    For Each prop In ActiveDocument.CustomDocumentProperties
        If prop.Name = "BIBDISP" Then
            If LCase(Left(prop.Value, 1)) = "k" Then UpdDisp = False        ' k means display keys
            Exit For
        End If
    Next
    If InStr(StyleFlags, ";") > 0 Then Comma = ";" Else Comma = ","
    If InStr(StyleFlags, ",") = 0 Then Comma = Comma & " "
    
    
    For i = 1 To 3                          ' 1=Document, 2=Footnotes, 3=Endnotes
        Select Case i
            Case 1
                nnotes = 1
            Case 2
                nnotes = ActiveDocument.Footnotes.Count
            Case 3
                nnotes = ActiveDocument.Endnotes.Count
        End Select
        If nnotes > 0 Then
            For j = 1 To nnotes
                Select Case i
                    Case 1
                        nfields = ActiveDocument.Fields.Count
                        If nfields > 0 Then
                            Set CiteField = ActiveDocument.Fields(1)
                        End If
                    Case 2
                        nfields = ActiveDocument.Footnotes(j).Range.Fields.Count
                        If nfields > 0 Then
                            Set CiteField = ActiveDocument.Footnotes(j).Range.Fields(1)
                            ActiveDocument.Footnotes(j).Range.Select
                        End If
                    Case 3
                        nfields = ActiveDocument.Endnotes(j).Range.Fields.Count
                        If nfields > 0 Then
                            Set CiteField = ActiveDocument.Endnotes(j).Range.Fields(1)
                            ActiveDocument.Endnotes(j).Range.Select
                        End If
                End Select
                If nfields >= 1 Then
                    While Not (CiteField Is Nothing)
                        NextCiteList (0)
                        If CiteRefs <> " " Then                 ' now update the list if necessary
                            Selection.start = CiteRange.start
                            Selection.End = CiteRange.End
                            WriteCiteList                       ' also deletes selection
                        End If
                    Wend
                End If
            Next j
        End If
    Next i
End Sub
Sub bibtex4word_style()
    DoneAction = 0                      ' reset action flags
    askstyle
    ' could avoid updating the bibliography if the style is unchanged
    If ActiveDocument.Bookmarks.Exists(BibBookmark) Then bibtex4word_bib   ' now update the bibliography
    Application.ScreenUpdating = True
End Sub
Private Sub askstyle()
    Dim prop As DocumentProperty
' We could find a list of installed styles by searching the registry
' for the miktex root %R from "My Computer\HKEY_LOCAL_MACHINE\SOFTWARE\MiK\MiKTeX|CurrentVersion\MikTeX\Install Root"
' and then finding the MiKTeX configuration file at %R\miktex\config\miktex.ini
' and then finding the [BibTeX] esection which contains an "Input Dirs" entry
' or alternatively we can check if a particular style exists by using kpsewhich
    Donaction = DoneAction And (Not DoneGetStyle)               ' need to requery style
    Set prop = getstyle
    ebibstyle = Environ("BIBSTYLE")
    styleprompt = "Enter name of bibtex style: plain, alpha, IEEEtran, ..."
    If (prop Is Nothing) And (ebibstyle = "") Then styleprompt = styleprompt & vbLf & "[You may wish to define an environment variable BIBSTYLE as a default style for future documents]"
    newstyle = ""
' need to cope better with cancelling the inputbox
    oldstyle = BibStyle
    If Not (StyleFlags = "") Then oldstyle = oldstyle & Mid(MagicChars, 2, 1) & StyleFlags
    If Not (MagicChars = MagicDefault) Then oldstyle = oldstyle & " " & MagicChars
    newstyle = InputBox(styleprompt, "Bibtex4Word - Set Style", oldstyle)
    If Len(newstyle) = 3 And Left(newstyle, 2) = "//" Then       ' command
        Select Case Right(newstyle, 1)
            Case "t"        ' show temp files
                If Dir(FileHead & ".aux") <> "" Then Shell ("notepad """ & FileHead & ".aux""")
                If Dir(FileHead & ".log") <> "" Then Shell ("notepad """ & FileHead & ".log""")
                If Dir(FileHead & ".bbl") <> "" Then Shell ("notepad """ & FileHead & ".bbl""")
                If Dir(FileHead & ".blg") <> "" Then Shell ("notepad """ & FileHead & ".blg""")
            Case "b"
                Shell ("cmd.exe /c """ & GetBibFile & ".bib""")
            Case Else
                MsgBox "Unknown command: " & newstyle
        End Select
    Else
        If newstyle <> "" Then
            If prop Is Nothing Then
                ActiveDocument.CustomDocumentProperties.Add Name:="BIBSTYLE", LinkToContent:=False, Value:=newstyle, Type:=msoPropertyTypeString
            Else
                prop.Value = newstyle
            End If
        End If
        Donaction = DoneAction And (Not DoneGetStyle)               ' need to requery style
        getstyle
    End If
End Sub
Sub bibtex4word_files()
    Dim fd As FileDialog
    Dim vrtSelectedItem As Variant
   
    DoneAction = 0                      ' reset action flags
    getstyle                            ' get style so we know whether to suppress path prefix
    zstyle = StyleNum(InStr(StyleFlags, "z"), StyleFlags)
    foundprop = False
    BibFile = ""
    docpath = ActiveDocument.Path
    For Each prop In ActiveDocument.CustomDocumentProperties
        If prop.Name = "BIBFILE" Then
            BibFile = prop.Value
            foundprop = True
            Exit For
        End If
    Next
    ebibfile = Environ("BIBFILE")
    If BibFile = "" Then BibFile = ebibfile
    bibpath = docpath
    bibname = BibFile
    If Len(BibFile) > 0 Then
        pos = InStrRev(BibFile, "\")
        If pos > 0 Then bibname = Mid(BibFile, pos + 1)
        If Mid(BibFile, 2, 1) = ":" Then
            bibpath = Left(BibFile, pos - 1)
        Else
            If pos > 0 Then bibpath = docpath & "\" & Left(BibFile, pos - 1)
        End If
    End If
    
    Set fd = Application.FileDialog(msoFileDialogFilePicker)
    With fd
        .Filters.Clear
        .Filters.Add "BibTex files", "*.bib"
        .AllowMultiSelect = False
        If ebibfile = "" Then
            If bibname = "" Then
                .Title = "Select BibTex file or set environment variable BIBFILE"
            Else
                .Title = "Current file is " & bibname & ", environment variable BIBFILE not set"
            End If
        Else
            If bibname = "" Then
                .Title = "Select BibTex file"
            Else
                .Title = "Current file is " & bibname
            End If
        End If
        If bibpath <> "" Then
            .InitialFileName = bibpath
        End If
        If .Show = -1 Then
            BibFile = .SelectedItems(1)
            If (zstyle And 2) = 0 And Len(BibFile) > Len(docpath) Then
                docpath = docpath & "\"
                If Left(BibFile, Len(docpath)) = docpath Then BibFile = Mid(BibFile, Len(docpath) + 1)
            End If
            If foundprop Then
                prop.Value = BibFile
            Else
                ActiveDocument.CustomDocumentProperties.Add Name:="BIBFILE", LinkToContent:=False, Value:=BibFile, Type:=msoPropertyTypeString
            End If
        End If
    End With
    Set fd = Nothing
    Application.ScreenUpdating = True
End Sub
Function GetBibFile()
    ' Determine the bibtex database. Returns string without the final .bib
    
    ebibfile = Environ("BIBFILE")
    For Each prop In ActiveDocument.CustomDocumentProperties
        If prop.Name = "BIBFILE" Then
            BibFile = prop.Value
            Exit For
        End If
    Next
    If BibFile & ebibfile = "" Then
        bibtex4word_files
        For Each prop In ActiveDocument.CustomDocumentProperties
            If prop.Name = "BIBFILE" Then
                BibFile = prop.Value
                Exit For
            End If
        Next
    End If
    If BibFile & ebibfile = "" Then
         MsgBox "You must specify a BibTex database"
    Else
        If BibFile = "" Then BibFile = ebibfile
        If Right(BibFile, 4) = ".bib" Then BibFile = Mid(BibFile, 1, Len(BibFile) - 4)
    End If
    If Mid(BibFile, 2, 1) <> ":" And BibFile <> "" Then                     ' check if a drive is specified
        BibFile = ActiveDocument.Path & "\" & BibFile
    End If
    GetBibFile = BibFile
End Function
Sub bibtex4word_save()
    DoneAction = 0                      ' reset action flags
    getstyle                                        ' set magic characters
    BibFile = GetBibFile & ".bib"
    If Dir(BibFile) = "" Then
        MsgBox ("Cannot open bibtex file: " & BibFile)
    Else
        CiteCount = 0
        If ActiveDocument.Fields.Count >= 1 Then
            Set myfield = ActiveDocument.Fields(1)
            While Not (myfield Is Nothing)
                myPos = InStr(1, myfield.Code, CitePrefix)                          ' chech for citation
                citepos = myPos
                If myPos = 0 Then myPos = InStr(1, myfield.Code, NoCitePrefix)      ' or hidden citation
                If (myfield.Type = wdFieldRef) And (myPos > 0) Then CiteCount = CiteCount + 1
                Set myfield = myfield.Next
            Wend
            If CiteCount = 0 Then
                MsgBox "No citations found in document"
            Else
            
                ' now extract the keys while eliminating duplicates
                
                ReDim keylist(1 To CiteCount) As String
                CiteCount = 0
                Set myfield = ActiveDocument.Fields(1)
                While Not (myfield Is Nothing)
                    myPos = InStr(1, myfield.Code, CitePrefix)                          ' chech for citation
                    citepos = myPos
                    If myPos = 0 Then myPos = InStr(1, myfield.Code, NoCitePrefix)      ' or hidden citation
                    If (myfield.Type = wdFieldRef) And (myPos > 0) Then
                        bibkey = Mid(myfield.Code, myPos + 4)
        
                        If InStr(bibkey, " ") > 0 Then
                            bibkey = Left(bibkey, InStr(bibkey, " ") - 1)
                        End If
                        If InStr(bibkey, "\") > 0 Then
                            bibkey = Left(bibkey, InStr(bibkey, "\") - 1)
                        End If
                        newkey = True
                        For i = 1 To CiteCount
                            If bibkey = keylist(i) Then
                                newkey = False
                                Exit For
                            End If
                        Next i
                        If newkey Then
                            CiteCount = CiteCount + 1
                            keylist(CiteCount) = bibkey
                        End If
                    End If
                    Set myfield = myfield.Next
                Wend
                Set fd = Application.FileDialog(msoFileDialogSaveAs)
                ' sadly, we cannot change the filters of the SaveAs dialog
                '  .Filters.Clear
                '  .Filters.Add "BibTex files", "*.bib"
                '  .AllowMultiSelect = False
                fd.Title = "Save BibTex File (extension is always .bib despite what is says below)"
                fd.FilterIndex = 4
                docname = ActiveDocument.FullName
                pos = InStrRev(docname, ".")
                If pos > 0 Then docname = Left(docname, pos - 1)
                fd.InitialFileName = ActiveDocument.FullName
                If fd.Show = -1 Then
                    outbib = fd.SelectedItems(1)
                    pos = InStr(outbib, ".")
                    If pos > 0 Then outbib = Left(outbib, pos - 1)
                    outbib = outbib & ".bib"
                    
                    ' check if we are ovewriting an existing file
                    
                    godoit = vbOK
                    If Dir(outbib) <> "" Then godoit = MsgBox("Overwrite existing " & outbib, vbOKCancel)
                    If godoit = vbOK Then
                        unopened = True
                        copyon = False
                        '
                        ' now read through bib file
                        '
                        Open BibFile For Input As #6
                        Do While Not EOF(6)    ' Loop until end of file
                            Line Input #6, textline    ' Read line into variable.
                            If Left(textline, 1) = "@" Then
                                If CiteCount = 0 Then Exit Do
                                copyon = False
                                pos = InStrRev(textline, "{")
                                If pos > 0 Then
                                    bibkey = Mid(textline, pos + 1, Len(textline) - pos - 1)
                                    For i = 1 To CiteCount
                                        If LCase(bibkey) = LCase(keylist(i)) Then
                                            copyon = True
                                            keylist(i) = keylist(CiteCount)
                                            CiteCount = CiteCount - 1
                                            Exit For
                                        End If
                                    Next i
                                End If
                            End If
                            If copyon Then
                                If unopened Then
                                    Open outbib For Output As #5
                                    unopened = False
                                End If
                                Print #5, textline
                            End If
                        Loop
                        Close #6
                        If unopened Then
                            MsgBox "None of the citations were found in BIB file"
                        Else
                            If CiteCount > 0 Then
                                If CiteCount > 1 Then
                                    MsgBox (CiteCount & " citations were not found")
                                Else
                                    MsgBox (CiteCount & " citation was not found")
                                End If
                                For i = 1 To CiteCount
                                    Print #5, """" & keylist(i) & """ was not found in database"
                                Next i
                            End If
                            Close #5
                        End If
                    End If
                End If
            End If
        Else
            MsgBox "No citations found in document"
        End If
        Close
    End If
    Application.ScreenUpdating = True
End Sub
Function getstyle() As DocumentProperty
    ' Determine style
    If (Not DoneAction) And DoneGetStyle Then            ' don't do anything if style cannot have changed
        BibStyle = ""                                       '  seems to be necessary to overrule previous value (weird)
        Set getstyle = Nothing
        For Each prop In ActiveDocument.CustomDocumentProperties
            If prop.Name = "BIBSTYLE" Then
                BibStyle = prop.Value
                Set getstyle = prop
                Exit For
            End If
        Next
        If BibStyle = "" Then BibStyle = Environ("BIBSTYLE")
        If BibStyle = "" Then BibStyle = "plain"
        '
        ' bibstyle has the format "style/flags magic"
        i = InStr(BibStyle, " ")
        MagicChars = MagicDefault                           ' default magic characters
        If i Then
            Mid(MagicChars, 1) = Mid(BibStyle, i + 1)
            BibStyle = Left(BibStyle, i - 1)
            ' now check that magic chars are non alphanumeric and distinct
            MagicChars = LCase(Replace(MagicChars, " ", ""))       ' remove any spaces
            magicbad = 0
            For j = 1 To Len(MagicChars)
                magicj = Mid(MagicChars, j, 1)
                If (magicj >= "a" And magicj <= "z") Or (magicj >= "0" And magicj <= "9") Then magicbad = 1
                If j > 1 Then
                    For k = 1 To j - 1
                        If magicj = Mid(MagicChars, k, 1) Then magicbad = 1
                    Next k
                End If
            Next j
        End If
        If magicbad Then
            MsgBox "Bad magic characters: """ & MagicChars & """ - reset to """ & MagicDefault & """"
            MagicChars = MagicDefault                           ' default magic characters
        End If
        i = InStr(BibStyle, Mid(MagicChars, 2, 1))
        If i Then
            StyleFlags = Mid(BibStyle, i + 1)
            BibStyle = Left(BibStyle, i - 1)
            If magicbad And Not (getstyle Is Nothing) Then getstyle.Value = BibStyle & Mid(MagicChars, 2, 1) & StyleFlags
        Else
            StyleFlags = ""
            If magicbad And Not (getstyle Is Nothing) Then getstyle.Value = BibStyle
        End If
        DoneAction = DoneAction Or DoneGetStyle             ' do not repeat this unnecessarily
    End If
End Function
Private Sub MakeCiteList()
'
' Scan citations in document and create an AUX file
'
    Dim myPos As Integer, bibkey As String, logging, TempDir As String
                 
    getstyle                                ' get the style to use
    ' Find the TEMP directory: Normally C:\Documents and Settings\***\Local Settings\Temp
    TempDir = Environ("BIBTEMP")
    If TempDir = "" Then
        TempDir = Environ("TEMP")
    End If
    If TempDir = "" Then
        TempDir = "."
    End If
    FileHead = TempDir & "\bibtex4word"     ' save filehead as a global variable
    If InStr(StyleFlags, "b") Then
        CiteCount = 1                    ' pretend we have at least one citation
    Else
        
        CiteCount = 0
        CiteList = ","
        For i = 1 To 3
            Select Case i
                Case 1
                    nnotes = 1
                Case 2
                    nnotes = ActiveDocument.Footnotes.Count
                Case 3
                    nnotes = ActiveDocument.Endnotes.Count
            End Select
            If nnotes > 0 Then
                For j = 1 To nnotes
                    Select Case i
                        Case 1
                            nfields = ActiveDocument.Fields.Count
                            If nfields > 0 Then
                                Set myfield = ActiveDocument.Fields(1)
                            End If
                        Case 2
                            nfields = ActiveDocument.Footnotes(j).Range.Fields.Count
                            If nfields > 0 Then
                                Set myfield = ActiveDocument.Footnotes(j).Range.Fields(1)
                            End If
                        Case 3
                            nfields = ActiveDocument.Endnotes(j).Range.Fields.Count
                            If nfields > 0 Then
                                Set myfield = ActiveDocument.Endnotes(j).Range.Fields(1)
                            End If
                    End Select
                    If nfields >= 1 Then
                        While Not (myfield Is Nothing)
                            myPos = InStr(1, myfield.Code, CitePrefix)
                            If myPos = 0 Then myPos = InStr(1, myfield.Code, NoCitePrefix)
                            If (myfield.Type = wdFieldRef) And (myPos > 0) Then
                                bibkey = Mid(myfield.Code, myPos + 4)
                
                                If InStr(bibkey, " ") > 0 Then
                                    bibkey = Left(bibkey, InStr(bibkey, " ") - 1)
                                End If
                                If InStr(bibkey, "\") > 0 Then
                                    bibkey = Left(bibkey, InStr(bibkey, "\") - 1)
                                End If
                                '
                                ' Now add to the citelist unless it is already there
                                '
                                decbibkey = decodekey(bibkey)
                                If InStr(CiteList, "," & decbibkey & ",") = 0 Then
                                    CiteList = CiteList & decbibkey & ","
                                    CiteCount = CiteCount + 1
                                End If
                            End If
                            Set myfield = myfield.Next
                        Wend
                    End If
                Next j
            End If
        Next i
        If CiteCount = 0 Then
            MsgBox "No citations found in document"
        Else
            GetBibFile
        End If
    End If                      ' if not the special style /b
End Sub
Private Sub createbib()

'
' bibtex4word_bib Macro
'        we assume that makecitelist has already been called
'
    Const Access As Long = &H100000
   
    Const BraceMax As Integer = 20
    Const ItalicFormat As Integer = 1, BoldFormat As Integer = 2, SmallCapFormat As Integer = 4
    Const SuperFormat As Integer = 8, DoiFormat As Integer = 16, UrlFormat As Integer = 32, VerbFormat As Integer = 64
    Const MathFormat As Integer = 128, SubFormat As Integer = 256, UnicharFormat As Integer = 512, CiteFormat As Integer = 1024
    Const CircChar As Long = 9675
    Const UHyphenOption As Integer = 2, UAbbrvOption As Integer = 4
    
    Dim iFile As Integer, bData() As Byte, lSize As Long
    Dim pstyle As Long
    Dim stylebits As Long
    Dim minxref As Long
    Dim nextline As String, bibline As String
    Dim myPos As Integer, bibkey As String, logging, TempDir As String
    Dim ProcessId As Long, ProcessHandle As Long
    Dim BraceLevel As Integer, BraceFormat(BraceMax) As Integer, BraceSpace(BraceMax)
    ' Unit names
    Dim UnitList As Variant
    UnitList = Array("em", "ex")    ' could include: "pt","pc","in","bp","cm","mm","dd","cc","sp" from TexBook p57
    ' Substitution strings
    Dim SubList As Variant
    ' the following substitutions are made before processing the bibtex output and cannot therefore be escaped
    SubList = Array("---", ChrW(&H2014), "--", ChrW(&H2013), "!`", ChrW(161), "?`", ChrW(191), _
                    "``", ChrW(&H201C), "''", ChrW(&H201D))
    ' Accent substitutions: line 0 gives Latex escape codes, remaining lines (one for each code) give original and accented character pairs
    Dim AndList As Variant
    AndList = Array("&", "and", "et", "en", "und") ' possible "and" words in author list
    Dim AccentList As Variant
    AccentList = Array("`'^""~cvu=.Hbd", _
                    "A" & ChrW(192) & "E" & ChrW(200) & "I" & ChrW(204) & "O" & ChrW(210) & "U" & ChrW(217) & "a" & ChrW(224) & "e" & ChrW(232) & "i" & ChrW(236) & "o" & ChrW(242) & "u" & ChrW(249), _
                    "A" & ChrW(193) & "E" & ChrW(201) & "I" & ChrW(205) & "O" & ChrW(211) & "U" & ChrW(218) & "a" & ChrW(225) & "e" & ChrW(233) & "i" & ChrW(237) & "o" & ChrW(243) & "u" & ChrW(250) & "Y" & ChrW(221) & "y" & ChrW(253), _
                    "A" & ChrW(194) & "E" & ChrW(202) & "I" & ChrW(206) & "O" & ChrW(212) & "U" & ChrW(219) & "a" & ChrW(226) & "e" & ChrW(234) & "i" & ChrW(238) & "o" & ChrW(244) & "u" & ChrW(251), _
                    "A" & ChrW(196) & "E" & ChrW(203) & "I" & ChrW(207) & "O" & ChrW(214) & "U" & ChrW(220) & "a" & ChrW(228) & "e" & ChrW(235) & "i" & ChrW(239) & "o" & ChrW(246) & "u" & ChrW(252) & "y" & ChrW(255) & "Y" & ChrW(&H178), _
                    "A" & ChrW(195) & "N" & ChrW(209) & "a" & ChrW(227) & "n" & ChrW(241), _
                    "C" & ChrW(199) & "c" & ChrW(231), _
                    "S" & ChrW(&H160) & "s" & ChrW(&H161) & "Z" & ChrW(&H17D) & "z" & ChrW(&H17E), _
                    "A" & ChrW(258) & "a" & ChrW(259), _
                    "A" & ChrW(256) & "a" & ChrW(257), _
                    "G" & ChrW(&H120) & "g" & ChrW(&H121), _
                    "O" & ChrW(336) & "o" & ChrW(337) & "U" & ChrW(368) & "u" & ChrW(369), _
                    "bb", _
                    "A" & ChrW(&H1EA0) & "a" & ChrW(&H1EA1) & "E" & ChrW(&H1EB8) & "e" & ChrW(&H1EB9) & "I" & ChrW(&H1ECA) & "i" & ChrW(&H1ECB) & "O" & ChrW(&H1ECC) & "o" & ChrW(&H1ECD) & "U" & ChrW(&H1EE4) & "u" & ChrW(&H1EE5) & "Y" & ChrW(&H1EF4) & "y" & ChrW(&H1EF5))
                    
   
    logging = True
    
    getstyle                                                ' set magic characters
    LogFileName = FileHead & ".log"
    AuxFileName = FileHead & ".aux"
    bblfilename = FileHead & ".bbl"
    BlgFileName = FileHead & ".blg"
    minxref = StyleNum(InStr(StyleFlags, "m"), StyleFlags, 0, 999)
    BibCommand = Environ("BIBEXE")
    If BibCommand = "" Then
        If InStr(StyleFlags, "+") > 0 Then
            BibCommand = "bibtex8"
        Else
            BibCommand = "bibtex"
        End If
    Else
        BibCommand = """" & BibCommand & """"
    End If
    If minxref > 0 Then
        BibCommand = BibCommand & " -min-crossrefs=" & CStr(minxref)
    End If
    BibCommand = BibCommand & " """ & FileHead & """"
    
    If logging Then
        Open LogFileName For Output As #2
        Print #2, "Bibtex4Word version: " & Version
        #If VBA7 Then
            Print #2, "VBA7=true"
        #Else
            Print #2, "VBA7=false"
        #End If
        #If Win64 Then
            Print #2, "WIN64=true"
        #Else
            Print #2, "WIN64=false"
        #End If
        Print #2, "Bibtex call: " & BibCommand

    End If
    
    stylefilebad = False
    bblstyle = InStr(StyleFlags, "b")
    If bblstyle > 0 Then                ' special style = use existing .BBL file
        bblfilename = ActiveDocument.FullName
        j = InStrRev(bblfilename, ".")
        If j = 0 Then
            MsgBox "File must be saved to use b style flag"
            Application.ScreenUpdating = True
            Error 1
        End If
        bblfilename = Mid(bblfilename, 1, j) & "bbl"
    Else
        If BibFile = "" Then
            MsgBox "No Bibtex data file specified"
            Application.ScreenUpdating = True
            Error 1
        End If
        Set fs = CreateObject("Scripting.FileSystemObject")
        If Not fs.fileexists(BibFile & ".bib") Then
            MsgBox BibFile & ".bib does not exist"
            Application.ScreenUpdating = True
            Error 1
        End If
        Set f = fs.GetFile(BibFile & ".bib")
        BibFile = f.shortpath
        BibFile = Left(BibFile, Len(BibFile) - 4)

        If logging Then
            Print #2, "AUX file: " & AuxFileName
            Print #2, "BIB file: " & BibFile
        End If

        Open AuxFileName For Output As #1    ' Open file for output.
        i = 2
        j = InStr(i, CiteList, ",")
        Do While j > 0
            Print #1, "\citation{" & Mid(CiteList, i, j - i) & "}"
            i = j + 1
            j = InStr(i, CiteList, ",")
        Loop
        Print #1, "\bibstyle{" & BibStyle & "}"
        Print #1, "\bibdata{" & BibFile & "}"
            
        If logging Then
            Print #2, CiteCount & " citations found in " & ActiveDocument.Name
        End If
        Close #1
    
            
            
        ' The code below is to create a blocking shell call
        ' See also: (for 32 bit programs) http://support.microsoft.com/kb/129796/EN-US/
        ' and (for 16 bit programs like Bibtex 0.99) http://support.microsoft.com/kb/96844/EN-US/
        
        execstat = ExecCmd(BibCommand)
        
        ' First analyse the log file from bibtex
        If execstat < 0 Then
            MsgBox "Bibtex call failed: " & BibCommand
            Application.ScreenUpdating = True
            Error 1 '
        End If
        
        If Dir(BlgFileName) = "" Then
            MsgBox "Missing Bibtex log file: " & BlgFileName
            Application.ScreenUpdating = True
            Error 1 '
        End If

        Open BlgFileName For Input As #4
        Do While Not EOF(4)    ' Loop until end of file
            Line Input #4, textline    ' Read line into variable.
            If InStr(textline, "couldn't open style file") > 0 Then
                stylefilebad = True
                Exit Do
            Else
                If InStr(textline, "The style file:") > 0 Then Exit Do
            End If
        Loop
    End If      ' not bbl style
    
    If stylefilebad Then
        MsgBox "Bibtex was unable to find style file: " & BibStyle
        If bblstyle = 0 Then
            Close #4
        End If
    Else                                    ' good style
    '
    '   Initialize the Bibliography
    '
        If ActiveDocument.Bookmarks.Exists(BibBookmark) Then
            ActiveDocument.Bookmarks(BibBookmark).Select
            Selection.InsertBefore vbLf         ' create a paragraph mark at the start to preserve the formatting style
            Selection.Font.Hidden = False       ' we must unhide it to avoid including the hidden attribute
            Selection.MoveStart Unit:=wdCharacter, Count:=1 ' don't delete it
            Selection.Delete
            Selection.MoveEnd Unit:=wdCharacter, Count:=-1 ' just before final LF
        Else
            If (Selection.Type = wdSelectionIP) Or (Selection.Type = wdSelectionNormal) Then
                Selection.Collapse Direction:=wdCollapseEnd
                Selection.InsertAfter vbLf
                stylemode = 0
                For Each sty In ActiveDocument.Styles
                    styname = LCase(sty.NameLocal)
                    If styname = "bibtex " & LCase(BibStyle) Then
                        stylemode = 2
                        styuse = sty
                        Exit For
                    ElseIf styname = "bibtex" Then
                        stylemode = 1
                        styuse = sty
                    End If
                Next sty
                If stylemode > 0 Then
                    Selection.Style = styuse
                End If
                ActiveDocument.Bookmarks.Add Name:=BibBookmark, Range:=Selection.Range
                Selection.MoveEnd Unit:=wdCharacter, Count:=-1 ' just before final LF
            Else
                MsgBox "Place cursor where you want the bibliography"
                Application.ScreenUpdating = True
                Error 1 ' should probably copy better with this
            End If
        End If
        numref = 0
        BibkeyOrder = " "                ' this will hold an ordered list of the encoded bibkeys
        If bblstyle = 0 Then ' Now add fake references for all missing ones
            Do While Not EOF(4)    ' Loop until end of BLG file
                Line Input #4, textline    ' Read line into variable.
                pos = InStr(textline, "I didn't find a database entry for")
                If pos > 0 Then
                    pos = InStr(textline, """")
                    bibkey = Mid(textline, pos + 1, InStr(pos + 1, textline, """") - pos - 1)
                    BibkeyEnc = encodekey(bibkey)
                    numref = numref + 1
                    Selection.InsertBefore "["
                    Selection.Font.Italic = False
                    Selection.Font.Bold = False
                    Selection.Font.SmallCaps = False
                    Selection.Collapse Direction:=wdCollapseEnd
                    Selection.InsertAfter (numref & "?")
                    ActiveDocument.Bookmarks.Add Name:=(CitePrefix & BibkeyEnc), Range:=Selection.Range
                    Selection.Collapse Direction:=wdCollapseEnd
                    ActiveDocument.Bookmarks.Add Name:=(NoCitePrefix & BibkeyEnc), Range:=Selection.Range
                    Selection.InsertAfter "]" & vbTab & """" & bibkey & """ not found in database" & vbLf
                    Selection.Collapse Direction:=wdCollapseEnd
                    BibkeyOrder = BibkeyOrder & BibkeyEnc & " "
                End If
            Loop
            Close #4
        End If          ' not bblstyle
     
        
        '   Process formatting flags
        
        If InStr(StyleFlags, ")") Then      ' /) = (1)
            OpenBracket = "("
            CloseBracket = ")"
        ElseIf InStr(StyleFlags, ".") Then      '/. = 1.
            OpenBracket = ""
            CloseBracket = "."
        ElseIf InStr(StyleFlags, "]") Then      ' /] = no brackets
            OpenBracket = ""
            CloseBracket = ""
        Else                                    ' default = [1]
            OpenBracket = "["
            CloseBracket = "]"
        End If
        If BibStyle = "plainnat" Or BibStyle = "abbrvnat" Or BibStyle = "unsrtnat" Or BibStyle = "elsarticle-harv" Then
            stylebits = 1       ' natbib format of bibitem
        ElseIf BibStyle = "chicago" Then
            stylebits = 2       ' chicago form of citeauthoryear
        Else
            stylebits = 0
        End If
        stylebits = StyleNum(InStr(StyleFlags, "#"), StyleFlags, stylebits, 0) ' special style features
        andstyle = StyleNum(InStr(StyleFlags, "&"), StyleFlags, 0, 0) ' URL processing options
        If (andstyle >= 0) And (andstyle <= UBound(AndList)) Then
            harvand = AndList(andstyle)
        Else
            harvand = "&"
        End If
        UStyle = StyleNum(InStr(StyleFlags, "u"), StyleFlags) ' URL processing options
        pstyle = StyleNum(InStr(StyleFlags, "p"), StyleFlags, 0, 65001) ' BBL file code page
        zstyle = StyleNum(InStr(StyleFlags, "z"), StyleFlags)
        hashstyle = StyleNum(InStr(StyleFlags, "#"), StyleFlags, 0, 1)  ' bibitem style
        UndoBlock = (zstyle And UndoZOpt) = 0               ' need to clear the undo buffer periodically
        'Year styles: 0=Smith (2000); 1=Smith; 2=Smith 2000; 3=Smith, 2000; 4=(2000); 5=2000
        YearStyle = StyleNum(InStr(StyleFlags, "y"), StyleFlags)
        i = InStr(StyleFlags, "~")
        If i > 0 Then                           ' /~ = space instead of tab
            LabelSep = ""
            N = StyleNum(i, StyleFlags)
            For i = 1 To N
                LabelSep = LabelSep & ChrW(160)  ' add n non-breaking spaces
            Next
        Else
            LabelSep = vbTab
        End If
        LabelLimit = 1000
        i = InStr(StyleFlags, "f")
        If i > 0 Then LabelLimit = StyleNum(i, StyleFlags)  ' /f = line feed after long label
        hidelabel = 0
        If InStr(StyleFlags, "l") Then          ' /l = Omit the label entirely
            hidelabel = 1
            OpenBracket = ""
            CloseBracket = ""
            LabelSep = ""
            LabelLimit = 1000
        End If
        
                 ' Process the BBL file line by line and copy into the bibliography
               
        If Dir(bblfilename) = "" Then
            MsgBox "Cannot open BBL file: " & bblfilename
            Application.ScreenUpdating = True
            Error 1 '
        End If
        '*******************************************
        ' Open bblfilename For Input As #3
        lSize = FileLen(bblfilename)
        'Debug.Print "FileLen = " & lSize
        If lSize > 0 Then
            ReDim bData(0 To lSize - 1)
            iFile = FreeFile()
            Open bblfilename For Binary As #iFile
            Get #iFile, , bData
            Close #iFile
            bbldata = BinToUni(bData, pstyle)
        Else
            MsgBox "Empty BBL file: " & bblfilename
            Application.ScreenUpdating = True
            Error 1 '
        End If
        '**************************************

        
        ' Process bibitems:
        '    We read lines one at a time and append them onto bibline until we have an entire bibitem
        '    Then we process the item and carry on reading the next line
        '
        biblinetype = -1            ' -1 = preamble, 0 = within bibitem, 1 = biblitem, 2 = end
        bibline = ""
        Application.ScreenUpdating = False
        bbli = 1
        bblj = InStr(bbli, bbldata, vbLf)
        Do While bblj > 0  ' Loop until end of file
           textline = Mid(bbldata, bbli, bblj - bbli) ' Read line into variable.
            If Left(textline, 21) = "\end{thebibliography}" Or Left(textline, 26) = "\end{mcitethebibliography}" Then
                biblinetype = 2
            Else
                If Left(textline, 8) = "\bibitem" Or Left(textline, 12) = "\harvarditem" Then biblinetype = 1      ' process the previous item before proceeding
            End If
            If biblinetype > 0 Then
            ' process bibline
                bibline = Trim(bibline)
                Do While InStr(bibline, "  ") > 0           ' collapse all multiple spaces
                    bibline = Replace(bibline, "  ", " ")
                Loop
                For i = LBound(SubList) To UBound(SubList) Step 2
                    bibline = Replace(bibline, SubList(i), SubList(i + 1))
                Next i
                If bibline <> "" Then
                
                    ' First initialize the bibliography line by typing "["
                
                    numref = numref + 1
                    EntryLen = 0
                    If Len(OpenBracket) Then
                        Selection.InsertBefore OpenBracket
                        Selection.Font.Italic = False
                        Selection.Font.Bold = False
                        Selection.Font.SmallCaps = False
                        Selection.Font.Hidden = False
                        EntryLen = 1
                    End If
                    Selection.Collapse Direction:=wdCollapseEnd
                    BkMkStart = Selection.start         ' remember the start of the selection
                    outformat = 0
                    nextformat = -1             ' -1 means just use old format after next {
                    doaccent = 0
                    BraceLevel = 0
                    BraceFormat(0) = 0
                    BraceSpace(0) = 0           ' ***** do we need this ?
                    forcespace = 0              ' -1 to skip spaces, +1 to force a space
                    gobblenum = 0               ' gobble an input number: 1=number, 3=number-unit, 15=number-unit + .. - ..
                    beginenv = False
                    endenv = False
                    mathmode = False
                    mathsupsub = 0              ' indicator that a pending super/subscript operation exists
                    lastformat = 0              ' remember previous format to insert a space after italic chars before superscripts
                    
                    ' Now extract the bibtex key and remove it from bibline
                    ' \harvarditem[optional short author list]{long author list]{year}{bibkey}
                    
                    If Left(bibline, 12) = "\harvarditem" Then
                        'harvstyle = 1  we don't seem to use this anywhere
                        nextbrace = InStr(13, bibline, "{")
                        nextbrack = InStr(13, bibline, "[")
                        harvshort = ""
                        If nextbrack > 0 And nextbrack < nextbrace Then ' find optional short author list
                            keystart = InStr(nextbrack + 1, bibline, "]{")
                            If keystart = 0 Then
                                keystart = InStr(nextbrack + 1, bibline, "] {")
                                If keystart = 0 Then
                                    MsgBox "Cannot find end of short author list"
                                    Application.ScreenUpdating = True
                                    Error 1 '
                                End If
                            End If
                            harvshort = Mid(bibline, 14, keystart - 14)
                            nextbrace = InStr(keystart + 1, bibline, "{")
                        End If
                        keystart = MatchBrace(nextbrace + 1, bibline) ' end of {long author list}
                        harvlong = Mid(bibline, nextbrace + 1, keystart - nextbrace - 1)
                        nextbrace = InStr(keystart + 1, bibline, "{")
                        keystart = MatchBrace(nextbrace + 1, bibline)   ' end of {year}
                        If keystart = 0 Then
                            MsgBox "Cannot find end of year"
                            Application.ScreenUpdating = True
                            Error 1 '
                        End If
                        harvyear = Mid(bibline, nextbrace + 1, keystart - nextbrace - 1)
                        keystart = InStr(keystart + 1, bibline, "{") + 1
                        If harvshort = "" Then
                            harvshort = harvlong
                        End If
                        If InStr(StyleFlags, "n") Then
                            Label = CStr(numref)     ' /n flag forces numeric
                        Else
                            If InStr(StyleFlags, "a") Then
                                Label = harvlong
                            Else
                                Label = harvshort
                            End If
                            Select Case YearStyle
                                Case 0
                                    Label = Label & " (" & harvyear & ")"
                                Case 2
                                    Label = Label & " " & harvyear
                                Case 3
                                    Label = Label & ", " & harvyear
                                Case 4
                                    Label = "(" & harvyear & ")"
                                Case 5
                                    Label = harvyear
                            End Select
                        End If
                    Else
                        nextbrace = InStr(9, bibline, "{")
                        nextbrack = InStr(9, bibline, "[")
                        If nextbrack > 0 And nextbrack < nextbrace Then
                            keystart = InStr(nextbrack + 1, bibline, "]{") + 2
                            If keystart < 3 Then
                                keystart = InStr(nextbrack + 1, bibline, "] {") + 3
                                If keystart < 4 Then
                                    MsgBox "Cannot find end of label"
                                    Application.ScreenUpdating = True
                                    Error 1 '
                                Else
                                    Label = Mid(bibline, 10, keystart - 13)
                                End If
                            Else
                                Label = Mid(bibline, 10, keystart - 12)
                            End If
                            If InStr(StyleFlags, "n") Then
                                Label = CStr(numref)     ' /n flag forces numeric
                            Else
                                ' If BibStyle = "plainnat" Or BibStyle = "abbrvnat" Or BibStyle = "unsrtnat" Or BibStyle = "elsarticle-harv" Then
                                If stylebits And 1 Then
                                    open1 = InStr(Label, "(")
                                    close1 = InStr(Label, ")")
                                    If open1 = 0 Or close1 = 0 Then
                                        MsgBox "Missing year in nat style"
                                        Application.ScreenUpdating = True
                                        Error 1
                                    End If
                                    If close1 - open1 = 1 Then
                                        harvyear = ""
                                    Else
                                        harvyear = Mid(Label, open1 + 1, close1 - open1 - 1)
                                    End If
                                    harvshort = Mid(Label, 1, open1 - 1) & labelyr
                                    If InStr(StyleFlags, "a") And Len(Label) > close1 + 1 Then
                                        Label = Mid(Label, close1 + 1) & labelyr
                                    Else
                                        Label = harvshort
                                    End If
                                    Select Case YearStyle
                                        Case 0
                                            Label = Label & " (" & harvyear & ")"
                                        Case 2
                                            Label = Label & " " & harvyear
                                        Case 3
                                            Label = Label & ", " & harvyear
                                        Case 4
                                            Label = "(" & harvyear & ")"
                                        Case 5
                                            Label = harvyear
                                    End Select
                                End If
                            End If
                        Else
                            Label = CStr(numref)
                            keystart = nextbrace + 1
                        End If
                    End If
                    keyend = InStr(keystart, bibline, "}")
                    bibkey = encodekey(Mid(bibline, keystart, keyend - keystart))
                    BibkeyOrder = BibkeyOrder & bibkey & " "
                    bibline = Label & vbLf & "]" & Mid(bibline, keyend + 1)
                    If logging Then
                        Print #2, "bibkey:" & bibkey & ", ref:[" & Label & "]:"
                    End If
                    
                    inlabel = 1
                    fromchar = 1
                    Outline = ""
                    '
                    ' We insert vbLf in front of each of the magic characters
                    ' so that we can find them easily
                    '
                    bibline = Replace(bibline, "\", vbLf & "\") ' escape sequence indicator
                    bibline = Replace(bibline, "{", vbLf & "{") ' start group
                    bibline = Replace(bibline, "}", vbLf & "}") ' end group
                    bibline = Replace(bibline, "$", vbLf & "$") ' start/end math mode
                    bibline = Replace(bibline, "_", vbLf & "_") ' subscript in math mode
                    bibline = Replace(bibline, "^", vbLf & "^") ' superscript in math mode
                    bibline = Replace(bibline, "\" & vbLf & "_", "\_") ' leave escaped versions alone
                    bibline = Replace(bibline, "\" & vbLf & "^", "\^") ' leave escaped versions alone


                    
                    ' We append vbLF & vbLF & "\ " to the line
                    ' the first vbLF forces outtext to be added to outline while
                    ' the second forces the resultant outline to be written into the document
                    ' and also forces a space before the next text sent to outline
                    
                    bibline = bibline & vbLf & vbLf & "\ "      ' append code to fill outtext and then flush buffer
                    lfindex = InStr(bibline, vbLf)
                    If lfindex > 0 Then                      '  _ and ^ are not special outside math mode
                        Do While Mid(bibline, lfindex + 1, 1) Like "[_^]"   ' unescape any _ or ^ at the start of the line
                            bibline = Mid(bibline, 1, lfindex - 1) & Mid(bibline, lfindex + 1) ' remove this vbLf escape character
                            lfindex = InStr(lfindex, bibline, vbLf)
                            If lfindex = 0 Then Exit Do
                        Loop
                    End If

                    Do While lfindex > 0        ' find all the vbLf codes in the line
                    
                        ' text to output is placed into the "outline" string which acts as an output buffer
                        ' Flush the output buffer if format has changed or if we are at the end of an input line or if the
                        ' first character of outtext needs to be super/subscripted
                        ' The format of outline is outformat; the format of outtext (the new stuff) is BraceFormat(BraceLevel)
                        
                        If BraceFormat(BraceLevel) <> outformat Or lfindex = Len(bibline) - 2 Or mathsupsub > 0 Then ' flush buffer
                            lenout = Len(Outline)
                            If lenout > 0 Then                            ' if there is anything to output
                                If ((outformat And UnicharFormat) > 0) Then        ' unicode character
                                    Outline = Replace(Outline, """", "&H")          ' convert hexadecimal prefix
                                    Outline = ChrW(Val(Outline))
                                    Outline = Replace(Outline, ChrW(0), "?")
                                    lenout = 1
                                End If
                                EntryLen = EntryLen + lenout              ' keep track of output chars for label length calculation
                                Selection.Collapse Direction:=wdCollapseEnd
                                If ((outformat And MathFormat) > 0) Then        ' mathformat: italicise letters only
                                    i = 1
                                    Do While i <= lenout
                                        For j = i To lenout
                                            If Mid(Outline, j, 1) Like "[A-Za-z]" Then
                                                Exit For
                                            End If
                                        Next j
                                        If j > i Then
                                            Selection.Collapse Direction:=wdCollapseEnd
                                            If (Mid(Outline, i, j - i) = ChrW(CircChar)) And ((outformat And SuperFormat) > 0) Then
                                                Selection.InsertAfter ChrW(176)          ' change ^\circ to degree sign
                                                Selection.Font.Superscript = False
                                            Else
                                                If ((outformat And SuperFormat) > 0) And ((lastformat And (ItalicFormat + SuperFormat + SubFormat)) = ItalicFormat) Then
                                                    Selection.InsertAfter " "        ' insert a space to compensate for previous italic character
                                                End If
                                                Selection.InsertAfter Mid(Outline, i, j - i)
                                                Selection.Font.Superscript = ((outformat And SuperFormat) > 0)
                                            End If
                                            Selection.Font.Italic = False
                                            Selection.Font.Bold = ((outformat And BoldFormat) > 0)
                                            Selection.Font.SmallCaps = ((outformat And SmallCapFormat) > 0)
                                            Selection.Font.Subscript = ((outformat And SubFormat) > 0)
                                            lastformat = outformat And Not ItalicFormat
                                            Selection.Font.Hidden = 0
                                            i = j
                                        End If
                                        If i <= lenout Then
                                            For j = i + 1 To lenout
                                                If Not (Mid(Outline, j, 1) Like "[A-Za-z]") Then
                                                    Exit For
                                                End If
                                            Next j
                                            Selection.Collapse Direction:=wdCollapseEnd
                                            If ((outformat And SuperFormat) > 0) And ((lastformat And (ItalicFormat + SuperFormat + SubFormat)) = ItalicFormat) Then
                                                Selection.InsertAfter " "        ' insert a space to compensate for previous italic character
                                            End If
                                            Selection.InsertAfter Mid(Outline, i, j - i)
                                            Selection.Font.Italic = True
                                            Selection.Font.Bold = ((outformat And BoldFormat) > 0)
                                            Selection.Font.SmallCaps = ((outformat And SmallCapFormat) > 0)
                                            Selection.Font.Superscript = ((outformat And SuperFormat) > 0)
                                            Selection.Font.Subscript = ((outformat And SubFormat) > 0)
                                            lastformat = outformat Or ItalicFormat
                                            Selection.Font.Hidden = 0
                                            i = j
                                        End If
                                    Loop
                                ElseIf (outformat And CiteFormat) > 0 Then
                                    CiteRefs = " ," & Outline & " "
                                    WriteCiteList
                                Else
                                    If ((outformat And SuperFormat) > 0) And ((lastformat And (ItalicFormat + SuperFormat + SubFormat)) = ItalicFormat) Then
                                        Selection.InsertAfter " "        ' insert a space to compensate for previous italic character
                                    End If
                                    Selection.InsertAfter Outline
                                    Selection.Font.Italic = ((outformat And ItalicFormat) > 0)
                                    Selection.Font.Bold = ((outformat And BoldFormat) > 0)
                                    Selection.Font.SmallCaps = ((outformat And SmallCapFormat) > 0)
                                    Selection.Font.Superscript = ((outformat And SuperFormat) > 0)
                                    Selection.Font.Subscript = ((outformat And SubFormat) > 0)
                                    Selection.Font.Hidden = 0
                                    lastformat = outformat
                                    If (outformat And UrlFormat) > 0 And (UStyle And 1) > 0 Then
                                        ActiveDocument.Hyperlinks.Add Address:=urllink, Anchor:=Selection.Range
                                    ElseIf (outformat And DoiFormat) > 0 And InStr(StyleFlags, "d") > 0 Then
                                        ActiveDocument.Hyperlinks.Add Address:="http://dx.doi.org/" & Outline, Anchor:=Selection.Range
                                    End If
                                End If
                                Outline = ""
                            End If
                            outformat = BraceFormat(BraceLevel)
                        End If
                        
                        ' add preceding text into the output buffer possibly with a preceding space
                        
                        While gobblenum > 0
                            If gobblenum And 1 Then         ' gobble up a floating point number [TexBook p57]
                                While Mid(bibline, fromchar, 1) = " "
                                    fromchar = fromchar + 1
                                Wend
                                If Mid(bibline, fromchar, 1) Like "[+-]" Then fromchar = fromchar + 1
                                While Mid(bibline, fromchar, 1) Like "[0-9]"
                                    fromchar = fromchar + 1
                                Wend
                                If Mid(bibline, fromchar, 1) Like "." Then
                                    fromchar = fromchar + 1
                                    While Mid(bibline, fromchar, 1) Like "[0-9]"
                                        fromchar = fromchar + 1
                                    Wend
                                End If
                                gobblenum = gobblenum And (Not 1)
                            End If
                            If gobblenum And 2 Then         ' gobble up a unit
                                While Mid(bibline, fromchar, 1) = " "
                                    fromchar = fromchar + 1
                                Wend
                                For i = LBound(UnitList) To UBound(UnitList)
                                    If InStr(fromchar, bibline, UnitList(i)) = fromchar Then
                                        fromchar = fromchar + Len(UnitList(i))
                                        Exit For
                                    End If
                                Next i
                                gobblenum = gobblenum And (Not 2)
                            End If
                            If gobblenum > 0 Then
                                While Mid(bibline, fromchar, 1) = " "
                                    fromchar = fromchar + 1
                                Wend
                                If (gobblenum And 4) And InStr(fromchar, bibline, "minus") = fromchar Then
                                        gobblenum = gobblenum - 1
                                        fromchar = fromchar + 5
                                ElseIf (gobblenum And 8) And InStr(fromchar, bibline, "plus") = fromchar Then
                                        gobblenum = gobblenum - 5
                                        fromchar = fromchar + 4
                                Else
                                    gobblenum = 0
                                End If
                            End If
                        Wend
                        While (forcespace <> 0) And Mid(bibline, fromchar, 1) = " "
                            fromchar = fromchar + 1
                        Wend
                        outtext = Mid(bibline, fromchar, lfindex - fromchar)
                        If forcespace > 0 Then
                            outtext = " " & outtext
                        End If
                        forcespace = 0
                        '
                        ' append outtext onto the end of outline after applying accents and tilde substitutions
                        '
                        If Len(outtext) > 0 Then
                            If doaccent > 0 And doaccent <= UBound(AccentList) Then  ' apply accent to the first char
                                pos = InStr(AccentList(doaccent), Left(outtext, 1))
                                If pos > 0 And (pos And 1) = 1 And Len(AccentList(doaccent)) > pos Then
                                    Mid(outtext, 1, 1) = Mid(AccentList(doaccent), pos + 1, 1)
                                    doaccent = 0
                                End If
                            End If
                            If (outformat And (UrlFormat + DoiFormat + VerbFormat)) = 0 Then
                                outtext = Replace(outtext, "~", ChrW(160))   ' insert non-breaking spaces except url/doi/verbatim formats
                                outtext = Replace(outtext, "`", ChrW(&H2018))   ' and use beautiful single quotes
                                outtext = Replace(outtext, "'", ChrW(&H2019))   '
                            ElseIf (outformat And UrlFormat) > 0 Then
                                urllink = outtext                           ' save original text for the url
                                If (UStyle And UAbbrvOption) > 0 Then
                                    outtext = "<url>"
                                ElseIf (UStyle And UHyphenOption) = 0 Then
                                    outtext = Replace(outtext, "/", "/" & Chr(31))  ' put an optional hyphen after each "/"
                                    outtext = Replace(outtext, "/" & Chr(31) & "/", "//") ' but not within a "//"
                                End If
                            End If
                            Outline = Outline & outtext
                        End If
                        If mathsupsub And Len(Outline) > 0 Then                                ' we need to super/subscript the first character of outline
                            If (mathsupsub = SuperFormat) And (Mid(Outline, 1, 1) = ChrW(CircChar)) Then
                                Mid(Outline, 1, 1) = ChrW(176)                                   ' convert ^\circ to degree sign
                            Else
                                Selection.Collapse Direction:=wdCollapseEnd
                                If (mathsupsub = SuperFormat) And ((lastformat And (ItalicFormat + SuperFormat + SubFormat)) = ItalicFormat) Then
                                    Selection.InsertAfter " "        ' insert a space to compensate for previous italic character
                                End If
                                Selection.InsertAfter Mid(Outline, 1, 1)        ' insert a character
                                Selection.Font.Italic = ((outformat And ItalicFormat) > 0) Or ((outformat And MathFormat) > 0) And (Mid(Outline, 1, 1) Like "[A-Za-z]")
                                Selection.Font.Bold = ((outformat And BoldFormat) > 0)
                                Selection.Font.SmallCaps = ((outformat And SmallCapFormat) > 0)
                                Selection.Font.Superscript = ((outformat And SuperFormat) > 0) Or ((mathsupsub And SuperFormat) > 0)
                                Selection.Font.Subscript = ((outformat And SuperFormat) = 0) And ((mathsupsub And SubFormat) > 0)
                                Selection.Font.Hidden = 0
                                Outline = Mid(Outline, 2)                       ' delete the first character
                            End If
                            lastformat = mathsupsub                         ' last format is definitely super/subscripted
                            mathsupsub = 0                                  ' reset the super/subscript flag
                        End If
                        
                        ' Now process the special characters \{}_^ as well as ] for end of label and vbLf for end of line
                        
                        fromchar = lfindex + 2      ' normally skip over the \,{,} or $ character following the vbLF
                        If Mid(bibline, lfindex + 1, 1) = "$" Then                  ' convert $...$ to {...} with MathFpormat set
                            If Mid(bibline, lfindex + 2, 2) = vbLf & "$" Then       ' treat $$ as $
                                fromchar = fromchar + 2
                            End If
                            If mathmode Then
                                Mid(bibline, fromchar - 1, 1) = "}"         ' end of MathFormat
                                mathmode = False
                            Else
                                nextformat = MathFormat
                                Mid(bibline, fromchar - 1, 1) = "{"         ' start of MathFormat
                                mathmode = True
                            End If
                        End If
                        Select Case Mid(bibline, fromchar - 1, 1)
                            Case vbLf
                                fromchar = fromchar - 1 ' don't skip the next vbLF because it is the true end of line
                            Case "]"                ' end of label
                                EntryLen = EntryLen + Len(Outline)
                                Selection.Collapse Direction:=wdCollapseEnd
                                Selection.InsertAfter Outline
                                Selection.Font.Italic = ((outformat And ItalicFormat) > 0)
                                Selection.Font.Bold = ((outformat And BoldFormat) > 0)
                                Selection.Font.SmallCaps = ((outformat And SmallCapFormat) > 0)
                                Selection.Font.Superscript = ((outformat And SuperFormat) > 0)
                                Selection.start = BkMkStart       ' restore the start of the bookmark
                                Selection.Font.Hidden = (hidelabel > 0) ' Hide the bookmark if required
                                ActiveDocument.Bookmarks.Add Name:=CitePrefix & bibkey, Range:=Selection.Range
                                Selection.Collapse Direction:=wdCollapseEnd
                                ActiveDocument.Bookmarks.Add Name:=NoCitePrefix & bibkey, Range:=Selection.Range
                                If EntryLen > LabelLimit Then
                                    Selection.InsertAfter CloseBracket & vbLf & LabelSep
                                Else
                                    Selection.InsertAfter CloseBracket & LabelSep
                                End If
                                EntryLen = 0
                                Selection.Font.Italic = 0
                                Selection.Font.Bold = 0
                                Selection.Font.SmallCaps = 0
                                Selection.Font.Superscript = 0
                                Selection.Font.Hidden = 0

                                Outline = ""    ' reset outline
                                outformat = 0
                                nextformat = -1
                                BraceLevel = 0
                                BraceFormat(0) = 0
                                BraceSpace(0) = 0
                                forcespace = -1              ' -1 to skip spaces, +1 to force a space
                                gobblenum = 0
                                inlabel = 0
                                beginenv = False
                                endenv = False
                            Case "_"                        ' we only come here in math mode anyway so no need to check
                                mathsupsub = SubFormat      ' subscript the next character or block
                            Case "^"
                                mathsupsub = SuperFormat    ' superscript the next character or block
                            Case "}"
                                BraceLevel = BraceLevel - 1
                                If BraceSpace(BraceLevel) = 1 Then
                                    BraceSpace(BraceLevel) = 0      ' reset flag
                                    forcespace = 1
                                End If

                            Case "{"
                                BraceLevel = BraceLevel + 1
                                If nextformat >= 0 Then
                                    BraceFormat(BraceLevel) = nextformat
                                Else
                                    BraceFormat(BraceLevel) = BraceFormat(BraceLevel - 1)
                                End If
                                nextformat = -1
                                If mathsupsub And ((BraceFormat(BraceLevel) And (SuperFormat + SubFormat)) = 0) Then     ' we need to super/subscript this block but only if normal already
                                    BraceFormat(BraceLevel) = BraceFormat(BraceLevel) + mathsupsub
                                    mathsupsub = 0                                  ' reset the super/subscript flag
                                End If
                                If beginenv Then
                                    beginenv = False
                                    i = fromchar
                                    fromchar = InStr(i, bibline, "}") - 1               ' skip to the matching brace
                                    envtype = Mid(bibline, i, fromchar - i)
                                    Select Case envtype
                                        Case "quotation", "quote"
                                            fromchar = fromchar - 1
                                            Mid(bibline, fromchar, 1) = Chr(11)         ' insert manual line break
                                        Case Else
                                            If logging Then
                                                Print #2, "*** Unknown environment:" & envtype
                                             End If
                                    End Select
                                End If
                                If endenv Then
                                    endenv = False
                                    i = fromchar
                                    fromchar = InStr(i, bibline, "}") - 1               ' skip to the matching brace
                                    envtype = LCase(Mid(bibline, i, fromchar - i - 1))
                                End If
    
                            Case "\"
                                ' find the next non-alphabetic character
                                i = lfindex + 1
                                Do
                                    i = i + 1
                                    ch = LCase(Mid(bibline, i, 1))
                                Loop While ch >= "a" And ch <= "z"
                                If i = lfindex + 2 Then
                                
                                ' single character latex command
                                
                                    fromchar = i + 1 ' normally skip over control char
                                    If logging Then
                                        If ch = vbLf Then
                                            Print #2, "latex command:" & Mid(bibline, i + 1, 1)
                                        Else
                                            Print #2, "latex command:" & ch
                                        End If
                                    End If
                                    Select Case ch
                                        Case " ", "&", "#", "$", "%", "_", "{", "}"          ' just print this char
                                            fromchar = i
                                        Case "`", "'", "^", """", "=", ".", "~"
                                            doaccent = InStr(AccentList(0), ch)
                                        Case vbLf                                     ' \{, \}, \\=new line, but ignore
                                        Case "/"                                      ' italic spacing correction: just accept following space
                                        Case Else
                                            If logging Then
                                                Print #2, "*** Unknown character command:" & ch
                                             End If
                                    End Select
                                Else
                                
                                ' multi character latex command
                                
                                    cmd = Mid(bibline, lfindex + 2, i - lfindex - 2)
                                    If logging Then
                                        Print #2, "latex command:" & cmd
                                    End If
                                    While Mid(bibline, i, 1) = " "
                                        i = i + 1       ' gobble any trailing spaces
                                    Wend
                                    newstr = ""         ' New string will go here
                                    Select Case cmd
                                        Case "bf"
                                            BraceFormat(BraceLevel) = BraceFormat(BraceLevel) Or BoldFormat
                                        Case "bgroup"           ' treat as {
                                            newstr = vbLf & "{"
                                        Case "begin"
                                            beginenv = True
                                        Case "bibAnnoteFile"            ' should check for a local file and insert annotation, but we just ignore it
                                            open1 = InStr(i, bibline, "{")                  ' start of argument 1
                                            i = MatchBrace(open1 + 1, bibline) + 1          ' gobble the entire argument
                                        Case "citeauthoryear"
                                            ' this command is used in "chicago" with three arguments and in "named" with only two
                                            open1 = InStr(i, bibline, "{")                  ' start of argument 1
                                            close1 = MatchBrace(open1 + 1, bibline)
                                            open2 = InStr(close1 + 1, bibline, "{")         ' start of argument 2
                                            close2 = MatchBrace(open2 + 1, bibline)
                                            If open1 = 0 Or close1 = 0 Or open2 = 0 Or close2 = 0 Then
                                                MsgBox "Bad argument for \citeauthoryear"
                                                Application.ScreenUpdating = True
                                                Error 1
                                            End If
                                            ' If BibStyle = "chicago" Then    ' skip over first parameter for "chicago" unless /a flag is present
                                            If stylebits And 2 Then    ' skip over first parameter for "chicago" unless /a flag is present
                                                open3 = InStr(close2 + 1, bibline, "{")         ' start of argument 3 [year]
                                                close3 = MatchBrace(open3 + 1, bibline)
                                                If open3 = 0 Or close3 = 0 Then
                                                    MsgBox "Bad argument for \citeauthoryear"
                                                    Application.ScreenUpdating = True
                                                    Error 1
                                                End If
                                                If InStr(StyleFlags, "a") = 0 Then    ' delete first argument
                                                    open1 = open2
                                                    close1 = close2
                                                End If
                                                open2 = open3
                                                close2 = close3
                                            End If
                                            harvshort = Mid(bibline, open1 + 1, close1 - open1 - 2)
                                            harvyear = Mid(bibline, open2 + 1, close2 - open2 - 2)
                                            Select Case YearStyle
                                                Case 0
                                                    harvshort = harvshort & " (" & harvyear & ")"
                                                Case 2
                                                    harvshort = harvshort & " " & harvyear
                                                Case 3
                                                    harvshort = harvshort & ", " & harvyear
                                                Case 4
                                                    harvshort = "(" & harvyear & ")"
                                                Case 5
                                                    harvshort = harvyear
                                            End Select
                                            bibline = harvshort & Mid(bibline, close2 + 1)
                                            i = 1
                                        Case "cite", "citet", "citep"
                                            nextformat = BraceFormat(BraceLevel) Or CiteFormat
                                        Case "end"
                                            endenv = True
                                        Case "doi"                  ' could turn this into a hyperlink
                                            newstr = "doi: "
                                            forcespace = 0
                                            nextformat = BraceFormat(BraceLevel) Or DoiFormat
                                        Case "em"
                                            BraceFormat(BraceLevel) = BraceFormat(BraceLevel) Xor ItalicFormat
                                        Case "emph"
                                            nextformat = BraceFormat(BraceLevel) Xor ItalicFormat
                                        Case "egroup"           ' treat as }
                                            If Mid(bibline, i, 1) = " " Then i = i + 1 ' skip following space
                                            newstr = vbLf & "}"
                                        Case "etal"
                                            Outline = Outline & "et al. "    ' insert "et al. " include space
                                        Case "etalchar"
                                            nextformat = BraceFormat(BraceLevel) Xor SuperFormat
                                        Case "harvardand"
                                            newstr = harvand
                                        Case "harvardyearleft"
                                            newstr = "("
                                        Case "harvardyearright"
                                            newstr = ")"
                                        Case "hskip"
                                            gobblenum = 15
                                            forcespace = 1
                                        Case "it"
                                            BraceFormat(BraceLevel) = BraceFormat(BraceLevel) Or ItalicFormat
                                        Case "kern", "lower"
                                            gobblenum = 3           ' gobble a number + a unit
                                        Case "latex"
                                            i = i - 5               ' Just output normally
                                        Case "mciteBstWouldAddEndPuncttrue"
                                            newstr = "."
                                        Case "penalty"
                                            gobblenum = 1           ' gobble a number without a unit
                                        Case "rm"
                                            BraceFormat(BraceLevel) = BraceFormat(BraceLevel) And (Not ItalicFormat)
                                        Case "sc"
                                            BraceFormat(BraceLevel) = BraceFormat(BraceLevel) Or SmallCapFormat
                                        Case "natexlab"             ' ignore if in label otherwise skip argument [plainnat]
                                            If inlabel = 0 Then
                                                open1 = InStr(i, bibline, "{")                  ' start of argument 1
                                                i = MatchBrace(open1 + 1, bibline) + 1          ' skip to matching brace
                                            End If
                                        Case "textbf"
                                            nextformat = BraceFormat(BraceLevel) Or BoldFormat
                                        Case "textit"
                                            nextformat = BraceFormat(BraceLevel) Or ItalicFormat
                                        Case "textsc"
                                            nextformat = BraceFormat(BraceLevel) Or SmallCapFormat
                                        Case "textrm"
                                            nextformat = BraceFormat(BraceLevel) And (Not ItalicFormat)
                                        Case "url"                  ' could turn this into a hyperlink
                                            nextformat = BraceFormat(BraceLevel) Or UrlFormat
                                        Case "verb"                                 ' verbatim
                                            ch = Mid(bibline, i, 1)                 ' delimiter character
                                            If ch = vbLf Then
                                                ch = Mid(bibline, i, 2)             ' delimiter may be two characters: e.g. vbLf+_
                                            End If
                                            j = InStr(i + Len(ch), bibline, ch) ' find matching delimiter
                                            If j = 0 Then
                                                MsgBox "Missing delimiter in \verb"
                                                Application.ScreenUpdating = True
                                                Error 1
                                            End If
                                            newstr = vbLf & "{" & Replace(Mid(bibline, i + Len(ch), j - i - Len(ch)), vbLf, "") & vbLf & "}" ' remove all escape characters
                                            i = j + Len(ch)
                                            nextformat = BraceFormat(BraceLevel) Or VerbFormat
                                        Case "newblock"             ' ignore because we have already had a space
                                        Case "protect"              ' ignore
                                        Case "relax"                ' ignore (used as forced terminator in Latex) should possibly reset some flags
                                        Case "unichar"              ' unicode character
                                            nextformat = BraceFormat(BraceLevel) Or UnicharFormat
                                        '
                                        ' Accents
                                        '
                                        Case "H", "c", "b", "d", "u", "v"
                                            doaccent = InStr(AccentList(0), cmd)
                                        Case "t"                                ' ignore tie accent
                                        '
                                        ' Special characters
                                        '
                                        Case "ae"
                                            newstr = ChrW(230)
                                        Case "AE"
                                            newstr = ChrW(198)
                                        Case "oe"
                                            newstr = ChrW(&H153) ' Chr(156) or ChrW(&H153)
                                        Case "OE"
                                            newstr = ChrW(&H152) ' Chr(140) or ChrW(&H152)
                                        Case "aa"
                                            newstr = ChrW(229)
                                        Case "AA"
                                            newstr = ChrW(197)
                                        Case "o"
                                            newstr = ChrW(248)
                                        Case "O"
                                            newstr = ChrW(216)
                                        Case "ss"
                                            newstr = ChrW(223)
                                        Case "aleph"
                                            newstr = ChrW(&H2135)
                                        Case "hbar"
                                            newstr = ChrW(&H127)
                                        Case "imath"
                                            newstr = ChrW(&H131)
                                        Case "jmath"
                                            newstr = ChrW(&H237)
                                        Case "ell"
                                            newstr = ChrW(&H2113)
                                        Case "wp"
                                            newstr = ChrW(&H2118)
                                        Case "Re"
                                            newstr = ChrW(&H211C)
                                        Case "Im"
                                            newstr = ChrW(&H2111)
                                        Case "prime"
                                            newstr = ChrW(&H2032)
                                        Case "nabla"
                                            newstr = ChrW(&H2207)
                                        Case "surd"
                                            newstr = ChrW(&H221A)
                                        Case "angle"
                                            newstr = ChrW(&H2220)
                                        Case "forall"
                                            newstr = ChrW(&H2200)
                                        Case "exists"
                                            newstr = ChrW(&H2203)
                                        Case "backslash"
                                            newstr = "\"
                                        Case "partial"
                                            newstr = ChrW(&H2202)
                                        Case "infty"
                                           newstr = ChrW(&H221E)
                                        Case "triangle"
                                            newstr = ChrW(&H2206)
                                        Case "Box"
                                            newstr = ChrW(&H25A1)
                                        Case "Diamond"
                                            newstr = ChrW(&H25C7)
                                        Case "flat"
                                            newstr = ChrW(&H266D)
                                        Case "natural"
                                            newstr = ChrW(&H266E)
                                        Case "sharp"
                                            newstr = ChrW(&H266F)
                                        Case "clubsuit"
                                            newstr = ChrW(&H2663)
                                        Case "diamondsuit"
                                            newstr = ChrW(&H2662)
                                        Case "heartsuit"
                                            newstr = ChrW(&H2661)
                                        Case "spadesuit"
                                            newstr = ChrW(&H2660)
                                        Case "dagger"
                                            newstr = ChrW(&H2020)
                                        Case "ddagger"
                                            newstr = ChrW(&H2021)
                                        Case "circ"
                                            newstr = ChrW(CircChar)      ' used subscripted as degree sign
                                        Case "eth"
                                            newstr = ChrW(&HF0)      ' Icelandic "th"
                                        Case "Eth"
                                            newstr = ChrW(&HD0)      ' Icelandic "Th"
                                        Case "copyright"
                                            newstr = ChrW(&HA9)
                                        Case "pounds"
                                            newstr = ChrW(&HA3)
                                        Case "dq"
                                            newstr = """"           ' double quote
                                        Case "pm"
                                            newstr = ChrW(179)       ' +-
                                        Case "mp"
                                            newstr = ChrW(8723)     ' -+
                                        Case "times"
                                            newstr = ChrW(215)       ' times
                                        Case "div"
                                            newstr = ChrW(247)       ' divide by
                                        Case "cdot"
                                            newstr = ChrW(183)       ' centre dot
                                        Case "sim"
                                            newstr = ChrW(&H223C)
                                            '
                                            ' Greek Letters
                                            '
                                        Case "Alpha"
                                            newstr = ChrW(913)
                                        Case "alpha"
                                            newstr = ChrW(945)
                                        Case "Beta"
                                            newstr = ChrW(914)
                                        Case "beta"
                                            newstr = ChrW(946)
                                        Case "Gamma"
                                            newstr = ChrW(915)
                                        Case "gamma"
                                            newstr = ChrW(947)
                                        Case "Delta"
                                            newstr = ChrW(916)
                                        Case "delta"
                                            newstr = ChrW(948)
                                        Case "Epsilon"
                                            newstr = ChrW(917)
                                        Case "epsilon"
                                            newstr = ChrW(949)
                                        Case "Zeta"
                                            newstr = ChrW(918)
                                        Case "zeta"
                                            newstr = ChrW(950)
                                        Case "Eta"
                                            newstr = ChrW(919)
                                        Case "eta"
                                            newstr = ChrW(951)
                                        Case "Theta"
                                            newstr = ChrW(920)
                                        Case "theta"
                                            newstr = ChrW(952)
                                        Case "vartheta"
                                            newstr = ChrW(&H3D1)
                                        Case "Iota"
                                            newstr = ChrW(921)
                                        Case "iota"
                                            newstr = ChrW(953)
                                        Case "Kappa"
                                            newstr = ChrW(922)
                                        Case "kappa"
                                            newstr = ChrW(954)
                                        Case "Lambda"
                                            newstr = ChrW(923)
                                        Case "lambda"
                                            newstr = ChrW(955)
                                        Case "Mu"
                                            newstr = ChrW(924)
                                        Case "mu"
                                            newstr = ChrW(956)
                                        Case "Nu"
                                            newstr = ChrW(925)
                                        Case "nu"
                                            newstr = ChrW(957)
                                        Case "Xi"
                                            newstr = ChrW(926)
                                        Case "xi"
                                            newstr = ChrW(958)
                                        Case "Omicron"
                                            newstr = ChrW(927)
                                        Case "omicron"
                                            newstr = ChrW(959)
                                        Case "Pi"
                                            newstr = ChrW(928)
                                        Case "pi"
                                            newstr = ChrW(960)
                                        Case "varpi"
                                            newstr = ChrW(&H3D6)
                                        Case "Rho"
                                            newstr = ChrW(929)
                                        Case "rho"
                                            newstr = ChrW(961)
                                        Case "varrho"
                                            newstr = ChrW(&H3F1)
                                        Case "Sigma"
                                            newstr = ChrW(931)
                                        Case "sigma"
                                            newstr = ChrW(963)
                                        Case "varsigma"                 ' terminal sigma
                                            newstr = ChrW(962)
                                        Case "Tau"
                                            newstr = ChrW(932)
                                        Case "tau"
                                            newstr = ChrW(964)
                                        Case "Upsilon"
                                            newstr = ChrW(933)
                                        Case "upsilon"
                                            newstr = ChrW(965)
                                        Case "Phi"
                                            newstr = ChrW(934)
                                        Case "phi"
                                            newstr = ChrW(966)
                                        Case "Chi"
                                            newstr = ChrW(935)
                                        Case "chi"
                                            newstr = ChrW(967)
                                        Case "Psi"
                                            newstr = ChrW(936)
                                        Case "psi"
                                            newstr = ChrW(968)
                                        Case "Omega"
                                            newstr = ChrW(937)
                                        Case "omega"
                                            newstr = ChrW(969)
                                        Case Else
                                            If logging Then
                                                Print #2, "*** Unknown command:" & cmd
                                             End If
                                    End Select
                                    If Len(newstr) Then
                                        fromchar = i - Len(newstr)
                                        Mid(bibline, fromchar, i - fromchar) = newstr       ' insert new string at front
                                    Else
                                        fromchar = i
                                    End If
                                End If
                        End Select
                        lfindex = InStr(fromchar, bibline, vbLf)
                        If Not mathmode And (lfindex > 0) Then                      '  _ and ^ are not special outside math mode
                            Do While Mid(bibline, lfindex + 1, 1) Like "[_^]"
                                bibline = Mid(bibline, 1, lfindex - 1) & Mid(bibline, lfindex + 1) ' remove this vbLf escape character
                                lfindex = InStr(lfindex, bibline, vbLf)             ' find the next vbLf
                                If lfindex = 0 Then Exit Do
                            Loop
                        End If
                    Loop            ' loop for each segment og the BBL line
                    Selection.InsertAfter vbLf
                    Selection.Collapse Direction:=wdCollapseEnd
                End If ' bibline <> ""
                If UndoBlock Then
                    ActiveDocument.UndoClear
                End If
                bibline = ""
                If biblinetype = 2 Then
                    If numref > 0 Then
                        Selection.MoveEnd Unit:=wdCharacter, Count:=1 ' select the final LF
                        Selection.Delete            ' and delete it
                    End If
                    Exit Do
                End If
            End If
            If biblinetype >= 0 Then
                If Right(bibline, 1) = "%" Then
                    If Right(bibline, 2) = "\%" Then
                        bibline = bibline & " " & textline
                    Else
                        bibline = Mid(bibline, 1, Len(bibline) - 1) & textline  ' omit a trailing % and the following space
                    End If
                Else
                    bibline = bibline & " " & textline      ' add textline onto the end of the current item
                End If
                biblinetype = 0
            End If
            bbli = bblj + 1
            bblj = InStr(bbli, bbldata, vbLf)
        Loop        ' loop for each line in the BBL file
    End If          ' ... else style file found by bibtex
    Close                           ' close all files
End Sub

Sub bibtex4word_bib()
    Dim oldsel As Word.Range
    Dim gtab As Table
    
    Application.ScreenUpdating = False
    DoneAction = 0                      ' reset action flags
    MakeCiteList ' Note that this also sets BibStyle, BibFile, Citelist and Citecount
    If BibFile = "" And InStr(StyleFlags, "b") = 0 Then
        MsgBox "No Bibtex data file specified"
    Else
        If CiteCount > 0 Then
            smartcut = Options.SmartCutPaste
            Options.SmartCutPaste = False
            Set oldsel = Selection.Range
            i = InStr(StyleFlags, "t")
            bibstyle0 = BibStyle
            styleflags0 = StyleFlags
            SlashChar = Mid(MagicChars, 2, 1)
            If i Then
            ' uses the last table in the document - could make this the table that contains the selection point
                N = StyleNum(i, StyleFlags)
                ntables = ActiveDocument.Tables.Count
                If ntables = 0 Then
                    MsgBox "Style=/t but no tables in document"
                Else
                    Set gtab = ActiveDocument.Tables(ntables)
                    ncols = gtab.Columns.Count
                    For rowi = N + 1 To gtab.Rows.Count
                        BibStyle = gtab.Cell(rowi, 1).Range.Text
                        BibStyle = Replace(Mid(BibStyle, 1, Len(BibStyle) - 1), vbCr, "")
                        i = InStr(BibStyle, " ")
                        If i = 0 And Len(BibStyle) > 0 Then         ' skip blanks and specs with spaces
                            i = InStr(BibStyle, SlashChar)
                            If i > 0 Then
                                StyleFlags = Mid(BibStyle, i + 1) & styleflags0
                                BibStyle = Left(BibStyle, i - 1)
                            Else
                                StyleFlags = styleflags0
                            End If
                            If BibStyle = "" Then BibStyle = bibstyle0
                            BibStyle = Trim(BibStyle)
                            If Len(BibStyle) > 0 Then
                                'MsgBox "style = " & BibStyle & " in row " & rowi & ", col " & ncols
                                If ActiveDocument.Bookmarks.Exists(BibBookmark) Then
                                    ActiveDocument.Bookmarks(BibBookmark).Delete
                                End If
                                gtab.Cell(rowi, ncols).Range.Select
                                Selection.Delete
                                Selection.Collapse Direction:=wdCollapseStart
                                createbib
                                Selection.MoveStart Unit:=wdCharacter, Count:=-1 ' select the final LF
                                Selection.Delete
                            End If
                        End If
                    Next rowi
                    oldsel.Select
                End If
            Else
                createbib
                bibrefresh
                oldsel.Select
            End If
            Options.SmartCutPaste = smartcut
        Else
            MsgBox "No citations found in document"
        End If
    End If
    Application.ScreenUpdating = True
End Sub


