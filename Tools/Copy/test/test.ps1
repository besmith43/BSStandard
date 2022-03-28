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
touch /home/testuser/test/10.txt
touch /home/testuser/test/11.txt

chown -R testuser:testuser /home/testuser/test
chown -R root:root /home/testuser/test/3
chmod -R 700 /home/testuser/test/3
chown root:root /home/testuser/test/10.txt
chmod 700 /home/testuser/test/10.txt

su -l testuser -c '/root/Copy /home/testuser/test /home/testuser/test2'

get-childitem /home/testuser/test | out-string | write-error
get-childitem /home/testuser/test2 | out-string | write-error

if (!(test-path /home/testuser/test2))
{
	write-error "Copy program didn't run"
}
elseif (!(test-path /home/testuser/test2/1))
{
	write-error "Copy program didn't copy folder 1"
}
elseif (!(test-path /home/testuser/test2/2))
{
	write-error "Copy program didn't copy folder 2"
}
elseif (!(test-path /home/testuser/test2/1/4.txt -PathType Leaf))
{
	write-error "Copy program didn't copy file 4"
}
elseif (!(test-path /home/testuser/test2/1/5.txt -PathType Leaf))
{
	write-error "Copy program didn't copy file 5"
}
elseif (!(test-path /home/testuser/test2/2/6.txt -PathType Leaf))
{
	write-error "Copy program didn't copy file 6"
}
elseif (!(test-path /home/testuser/test2/2/7.txt -PathType Leaf))
{
	write-error "Copy program didn't copy file 7"
}

if (test-path /home/testuser/test2/3)
{
	write-error "Copy program copied folder 3"
}
elseif (test-path /home/testuser/test2/3/8.txt -PathType Leaf)
{
	write-error "Copy program copied file 8"
}
elseif (test-path /home/testuser/test2/3/9.txt -PathType Leaf)
{
	write-error "Copy program copied file 9"
}

if (test-path /home/testuser/test2/10.txt -PathType Leaf)
{
	write-error "Copy program copied file 10"
}
elseif (!(test-path /home/testuser/test2/11.txt -PathType Leaf))
{
	write-error "Copy program didn't copy file 11"
}