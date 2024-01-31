# Lấy thư mục chứa các model
$outputDirectory = "CodeBE_LEM/Models"

# Tìm kiếm tất cả file .cs trong thư mục
$files = Get-ChildItem -Path $outputDirectory -Filter *.cs

#Lấy tên của các class
$classNames = foreach ($file in $files) { $file.Name.Substring(0, $file.Name.Length - 3) }

foreach ($file in $files) {   
    # Bỏ qua file DataContext.cs
    if ($file.Name -eq "DataContext.cs") {
        continue
    }
    # Đọc nội dung file
    $content = Get-Content -Path $file.FullName

    # Lấy tên class từ tên file
    $className = $file.Name.Substring(0, $file.Name.Length - 3)

    # Thay đổi tên class
    $content = $content.Replace($className, $className + "DAO")

    # Tạo tên file mới
    $newFileName = $className + "DAO.cs"

    # Ghi đè nội dung file
    Set-Content -Path $file.FullName -Value $content 
    Rename-Item -Path $file.FullName -NewName $newFileName
}

# Thông báo thành công
Write-Host "Đã thay đổi tên class thành công!"
