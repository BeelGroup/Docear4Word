namespace Docear4Word
{
	public static class SymbolHelper
	{
		public static char GetAccentedChar(char accentType, char toAccent)
		{
			switch(accentType)
			{
				case '`':
					switch(toAccent)
					{
						case 'A': return 'À';
						case 'E': return 'È';
						case 'I': return 'Ì';
						case 'O': return 'Ò';
						case 'U': return 'Ù';
						case 'a': return 'à';
						case 'e': return 'è';
						case 'i': return 'ì';
						case 'o': return 'ò';
						case 'u': return 'ù';
					}
					break;

				case '\'':
					switch(toAccent)
					{
						case 'A': return 'Á';
						case 'E': return 'É';
						case 'I': return 'Í';
						case 'O': return 'Ó';
						case 'U': return 'Ú';
						case 'a': return 'á';
						case 'e': return 'é';
						case 'i': return 'í';
						case 'o': return 'ó';
						case 'u': return 'ú';
						case 'Y': return 'Ý';
						case 'y': return 'ý';
					}
					break;

				case '^':
					switch(toAccent)
					{
						case 'A': return 'Â';
						case 'E': return 'Ê';
						case 'I': return 'Î';
						case 'O': return 'Ô';
						case 'U': return 'Û';
						case 'a': return 'â';
						case 'e': return 'ê';
						case 'i': return 'î';
						case 'o': return 'ô';
						case 'u': return 'û';
					}
					break;

				case '\"':
					switch(toAccent)
					{
						case 'A': return 'Ä';
						case 'E': return 'Ë';
						case 'I': return 'Ï';
						case 'O': return 'Ö';
						case 'U': return 'Ü';
						case 'a': return 'ä';
						case 'e': return 'ë';
						case 'i': return 'ï';
						case 'o': return 'ö';
						case 'u': return 'ü';
						case 'y': return 'ÿ';
						case 'Y': return 'Ÿ';

						case 's': return 'ß'; // Not in spec as far as I can see but appeared in Docear database
					}
					break;

				case '~':
					switch(toAccent)
					{
						case 'A': return 'Ã';
						case 'N': return 'Ñ';
						case 'a': return 'ã';
						case 'n': return 'ñ';
					}
					break;

				case 'c':
					switch(toAccent)
					{
						case 'C': return 'Ç';
						case 'c': return 'ç';
					}
					break;

				case 'v':
					switch(toAccent)
					{
						case 'S': return 'Š';
						case 's': return 'š';
						case 'Z': return 'Ž';
						case 'z': return 'ž';
					}
					break;

				case 'u':
					switch(toAccent)
					{
						case 'A': return 'Ă';
						case 'a': return 'ă';
					}
					break;

				case '=':
					switch(toAccent)
					{
						case 'A': return 'Ā';
						case 'a': return 'ā';
					}
					break;

				case '.':
					switch(toAccent)
					{
						case 'G': return 'Ġ';
						case 'g': return 'ġ';
					}
					break;

				case 'H':
					switch(toAccent)
					{
						case 'O': return 'Ő';
						case 'o': return 'ő';
						case 'U': return 'Ű';
						case 'u': return 'ű';
					}
					break;

				case 'b':
					break;

				case 'd':
					switch(toAccent)
					{
						case 'A': return 'Ạ';
						case 'a': return 'ạ';
						case 'E': return 'Ẹ';
						case 'e': return 'ẹ';
						case 'I': return 'Ị';
						case 'i': return 'ị';
						case 'O': return 'Ọ';
						case 'o': return 'ọ';
						case 'U': return 'Ụ';
						case 'u': return 'ụ';
						case 'Y': return 'Ỵ';
						case 'y': return 'ỵ';
					}
					break;


			}

			return '\0';
		}

		public static string GetReplacementFor(string word)
		{
			switch(word)
			{
				case "bgroup": return "{";

				case "bf":
				case "begin":
				case "bibAnnoteFile":
				case "end":
                case "em":
                case "emph":
				case "textbf":
				case "textit":
				case "textsc":
				case "textrm":
				case "url":
				case "newblock":
				case "protect":
				case "relax":
				case "rm":
				case "etalchar":
                case "sc":
				case "it":
				case "cite":
				case "citet":
				case"citep":
					return string.Empty;

				case "harvardyearleft": return "(";
				case "harvardyearright": return ")";
				case "mciteBstWouldAddEndPuncttrue": return ".";
				case "latex": return word;
				case "harvardand": return "&";
                case "etal": return "et al. ";


/*
                                            open1 = InStr(i, bibline, "{")                  ' start of argument 1
                                            i = MatchBrace(open1 + 1, bibline) + 1          ' gobble the entire argument
                                        case "citeauthoryear"
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
                                            Select case YearStyle
                                                case 0
                                                    harvshort = harvshort & " (" & harvyear & ")"
                                                case 2
                                                    harvshort = harvshort & " " & harvyear
                                                case 3
                                                    harvshort = harvshort & ", " & harvyear
                                                case 4
                                                    harvshort = "(" & harvyear & ")"
                                                case 5
                                                    harvshort = harvyear
                                            End Select
                                            bibline = harvshort & Mid(bibline, close2 + 1)
                                            i = 1
                                        case "doi"                  ' could turn this into a hyperlink
                                            newstr = "doi: "
                                            forcespace = 0
                                            nextformat = BraceFormat(BraceLevel) Or DoiFormat
                                        case "egroup"           ' treat as }
                                            If Mid(bibline, i, 1) = " " Then i = i + 1 ' skip following space
                                            newstr = vbLf & "}"
                                        case "hskip"
                                            gobblenum = 15
                                            forcespace = 1
                                        case "kern", "lower"
                                            gobblenum = 3           ' gobble a number + a unit
                                        case "penalty"
                                            gobblenum = 1           ' gobble a number without a unit
                                        case "natexlab"             ' ignore if in label otherwise skip argument [plainnat]
                                            If inlabel = 0 Then
                                                open1 = InStr(i, bibline, "{")                  ' start of argument 1
                                                i = MatchBrace(open1 + 1, bibline) + 1          ' skip to matching brace
                                            End If
                                        case "verb"                                 ' verbatim
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
                                        case "unichar"              ' unicode character
                                            nextformat = BraceFormat(BraceLevel) Or UnicharFormat
*/
				case "ae": return "æ";
				case "AE": return "Æ";
				case "oe": return "œ";
				case "OE": return "Œ";
                case "aa": return "å";
                case "AA": return "Å";
                case "o": return "ø";
                case "O": return "Ø";
                case "ss": return "ß";
                case "aleph": return "ℵ";
                case "hbar": return "ħ";
                case "imath": return "ı";
                case "jmath": return "ȷ";
                case "ell": return "ℓ";
                case "wp": return "\x2118";
                case "Re": return "ℜ";
                case "Im": return "ℑ";
                case "prime": return "′";
                case "nabla": return "\x2207";
                case "surd": return "\x221a";
                case "angle": return "\x2220";
                case "forall": return "\x2200";
                case "exists": return "\x2203";
                case "backslash": return "\\"; 
                case "partial": return "\x2202";
                case "infty": return "\x221e";
                case "triangle": return "\x2206";
                case "Box": return "\x25a1";
                case "Diamond": return "\x25c7";
                case "flat": return "\x266d";
                case "natural": return "\x266e";
                case "sharp": return "\x266f";
                case "clubsuit": return "\x2663";
                case "diamondsuit": return "\x2662";
                case "heartsuit": return "\x2661";
                case "spadesuit": return "\x2660";
                case "dagger": return "†";
                case "ddagger": return "‡";
                case "circ": return "\xb0";
                case "eth": return "ð";
                case "Eth": return "Ð";
                case "copyright": return "©";
                case "pounds": return "£";
                case "dq": return "\"";
                case "pm": return "±";
                case "mp": return "\x2213";
                case "times": return "×";
                case "div": return "÷";
                case "cdot": return "·";
                case "sim": return "\x223c";
                case "Alpha": return "Α";
                case "alpha": return "α";
                case "Beta": return "Β";
                case "beta": return "β";
                case "Gamma": return "Γ";
                case "gamma": return "γ";
                case "Delta": return "Δ";
                case "delta": return "δ";
                case "Epsilon": return "Ε";
                case "epsilon": return "ε";
                case "Zeta": return "Ζ";
                case "zeta": return "ζ";
                case "Eta": return "Η";
                case "eta": return "η";
                case "Theta": return "Θ";
                case "theta": return "θ";
                case "vartheta": return "ϑ";
                case "Iota": return "Ι";
                case "iota": return "ι";
                case "Kappa": return "Κ";
                case "kappa": return "κ";
                case "Lambda": return "Λ";
                case "lambda": return "λ";
                case "Mu": return "Μ";
                case "mu": return "μ";
                case "Nu": return "Ν";
                case "nu": return "ν";
                case "Xi": return "Ξ";
                case "xi": return "ξ";
                case "Omicron": return "Ο";
                case "omicron": return "ο";
                case "Pi": return "Π";
                case "pi": return "π";
                case "varpi": return "ϖ";
                case "Rho": return "Ρ";
                case "rho": return "ρ";
                case "varrho": return "ϱ";
                case "Sigma": return "Σ";
                case "sigma": return "σ";
                case "varsigma": return "ς";
                case "Tau": return "Τ";
                case "tau": return "τ";
                case "Upsilon": return "Υ";
                case "upsilon": return "υ";
                case "Phi": return "Φ";
                case "phi": return "φ";
                case "Chi": return "Χ";
                case "chi": return "χ";
                case "Psi": return "Ψ";
                case "psi": return "ψ";
                case "Omega": return "Ω";
                case "omega": return "ω";

				case "textbackslash": return "\\";
				case "textendash": return "–";
				case "textemdash": return "—";
				case "S": return "§";
				case "P": return "¶";
				case "textasciicircum": return "^";
				case "textasciitilde": return "~";
				case "ldots": return "…";
				case "textquestiondown": return "¿";
				case "textregistered": return "®";
				case "textexclamdown": return "¡";
				case "texttrademark": return "\x2122";
				case "textquoteright": return "’";
				case "textquotedblright": return "”";
				case "textquoteleft": return "‘";
				case "textquotedblleft": return "“";
				case "yen": return "¥";
				case "i": return "\uD835\uDEA4";
				case "j": return "\uD835\uDEA5";
				case "textbar": return "|";
				case "textperiodcentered": return "·";
				case "textless": return "<";
				case "textgreater": return ">";
				case "slash": return "/";
			}

			return null;
		}
	}
}