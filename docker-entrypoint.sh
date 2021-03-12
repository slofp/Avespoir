#!/bin/sh

date "+>>> %Y/%m/%d <<<" >> ./Log/bash_log.log

echo "Starting Avespoir..." >> ./Log/bash_log.log
./Avespoir &
app_pid=$!
echo "Started Avespoir" >> ./Log/bash_log.log

echo "Process PID: $app_pid" >> ./Log/bash_log.log

ExitEvent() {
	echo "=== ExitEvent Start ===" >> ./Log/bash_log.log
	echo "Signal: $1" >> ./Log/bash_log.log

	echo "Call kill: $app_pid" >> ./Log/bash_log.log
	kill ${app_pid}
	echo "Waiting: $app_pid" >> ./Log/bash_log.log
	wait ${app_pid}
	echo "Killed process." >> ./Log/bash_log.log

	echo "Copy database file..." >> ./Log/bash_log.log
	cp --backup=numbered ./*.db ./.DB_Backup
	echo "Copyed database file." >> ./Log/bash_log.log

	echo "=== ExitEvent End ===" >> ./Log/bash_log.log
	exit 0
}

trap "ExitEvent TERM" TERM
trap "ExitEvent KILL" KILL
trap "ExitEvent INT" INT
trap "ExitEvent QUIT" QUIT

wait ${app_pid}

echo "Exit?" >> ./Log/bash_log.log

exit 1
