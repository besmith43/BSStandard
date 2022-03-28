useradd -m testuser

mkdir /home/testuser/test

mkdir /home/testuser/test/1
mkdir /home/testuser/test/2
mkdir /home/testuser/test/3

touch /home/testuser/test/1/4.txt
touch /home/testuser/test/1/5.txt
touch /home/testuser/test/2/6.txt
touch /home/testuser/test/2/7.txt
touch /home/testuser/test/3/8.txt
touch /home/testuser/test/3/9.txt

chown -R testuser:testuser /home/testuser/test

su -l testuser -c '/root/DiffFolder /home/testuser/test /home/testuser/test'

Copy-Item -r /home/testuser/test /home/testuser/test2

su -l testuser -c '/root/DiffFolder /home/testuser/test /home/testuser/test2'

chown -R root:root /home/testuser/test/3
chmod -R 700 /home/testuser/test/3

su -l testuser -c '/root/DiffFolder /home/testuser/test /home/testuser/test2'