Setting bootrap: 
- ng add ngx-bootstrap
Setting Font
- npm install font-awesome
- Trong style của file angular.json thêm "node_modules/font-awesome/css/font-awesome.min.css"
Setting https://localhost:4200 phía client
- link git: https://github.com/FiloSottile/mkcert?tab=readme-ov-file (cái này đã tải trong máy)
- mkdir ssl (Tạo file ssl) tạo xong chuyển cmd client về: cd ssl
- mkcert -install (kiểm tra đã cài đặt chưa)
- mkcert localhost để tạo các file liên quan
- trong serve của angular.json thêm vào:
        "options": {
            "ssl": true,
            "sslCert": "./ssl/localhost.pem",
            "sslKey": "./ssl/localhost-key.pem"
          },
Setting file environments để tạo http lấy dữ liệu bên server: ng g environments
Thư viện upload file: https://valor-software.com/ng2-file-upload/
Thư viện ảnh gallery: https://ngx-gallery.netlify.app/#/gallery
  
