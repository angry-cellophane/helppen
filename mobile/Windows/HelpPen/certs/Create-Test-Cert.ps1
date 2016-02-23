$dnsName = "helppen.com"
$friendlyName = "HelpPen"
$certStorePath = "cert:\localmachine\my"
$certPassword = "123"


# Создаем корневой сертификат для установки в доверенные корневые центры сертификации
$cert = New-SelfSignedCertificate `
            -CertStoreLocation $certStorePath `
            -FriendlyName $friendlyName `
            -KeyUsage DigitalSignature,CertSign,CRLSign,DataEncipherment,KeyAgreement,KeyEncipherment `
            -KeyUsageProperty All `
            -Type CodeSigningCert `
            -Subject "CN=Help Pen" `
            -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.3") `
            -KeyExportPolicy Exportable `
            -NotAfter "2017-02-22"

# Сохраняем его в файл
$certPasswordSecure = ConvertTo-SecureString -String $certPassword -Force –AsPlainText
Export-PfxCertificate -Cert $cert -FilePath "HelpPen.com.pfx" -Password $certPasswordSecure

