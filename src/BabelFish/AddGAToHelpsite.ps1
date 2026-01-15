###############################################################################
#
# Literally just adding google anaylitics to all the helpsite pages.
# 
###############################################################################


$folder = "C:\temp\Help\html\"
$snippet = @"
<!-- Google Analytics -->
<script async src="https://www.googletagmanager.com/gtag/js?id=G-DQKZE9Y838"></script>
<script>
    window.dataLayer = window.dataLayer || [];
    function gtag(){dataLayer.push(arguments);}
    gtag('js', new Date());
    gtag('config', 'G-DQKZE9Y838');
</script>
"@

Get-ChildItem -Path $folder -Filter *.htm -Recurse | ForEach-Object {
    $content = Get-Content $_.FullName -Raw

    # Insert snippet before </head>
    if ($content -match "</head>") {
        $newContent = $content -replace "</head>", "$snippet`r`n</head>"
        Set-Content -Path $_.FullName -Value $newContent -Encoding UTF8
        #Write-Host "Updated $($_.FullName)"
    }
}
