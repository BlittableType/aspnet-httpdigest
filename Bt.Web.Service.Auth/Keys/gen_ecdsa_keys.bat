openssl ecparam -genkey -name secp521r1 > priv-key.pem
openssl ec -in priv-key.pem -pubout > pub-key.pem