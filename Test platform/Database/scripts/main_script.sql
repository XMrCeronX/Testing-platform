-- Скрипт на создание базы данных "Платформа для тестирования"

USE `sys`;

DROP DATABASE IF EXISTS `test_platform`;
CREATE DATABASE `test_platform`;

USE `test_platform`;

DROP TABLE IF EXISTS `roles`;
CREATE TABLE `roles` (
	`id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	`name` VARCHAR(255) NOT NULL UNIQUE -- имя роли
);

DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
	`id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	`name` VARCHAR(255) NOT NULL, -- имя
	`surname` VARCHAR(255) NOT NULL, -- фамилия
	`patronymic` VARCHAR(255) NOT NULL, -- отчество
	`password` VARCHAR(255) NOT NULL, -- пароль
	`role_id` BIGINT NOT NULL, -- роль
	`time_created` TIMESTAMP NOT NULL DEFAULT NOW(), -- дата создания пользователя
	`time_last_modified` TIMESTAMP NOT NULL DEFAULT NOW(), -- дата последнего изменения пользователя
	FOREIGN KEY (role_id) REFERENCES `roles`(id)
);

INSERT INTO `roles` (name)
VALUES
('Администратор'),
('Учитель'),
('Студент');

INSERT INTO `users` (patronymic, name, surname, role_id, `password`)
VALUES
('гуляй вася', 'root', 'Не твоего ума дело',1, MD5('куку_мой_сладкий')),
('Симонова', 'Ксения', 'Ивановна',2,MD5('OWJ895OVXV')),
('Старостина', 'Елизавета', 'Фёдоровна',2,MD5('I9F3DNHPWL')),
('Васильев', 'Святослав', 'Михайлович',2,MD5('ISUWSC7KI3')),
('Лазарева', 'Варвара', 'Кирилловна',2,MD5('GAMZCLFZGX')),
('Тарасов', 'Даниил', 'Никитич',2,MD5('MBUJ90D2TE')),
('Сафонов', 'Алексей', 'Никитич',3,MD5('0772AYSGLV')),
('Филимонов', 'Данила', 'Даниилович',3,MD5('C0S3OV69BW')),
('Фомичева', 'Айлин', 'Александровна',3,MD5('U8GIOPNR42')),
('Николаева', 'Виктория', 'Дмитриевна',3,MD5('DUCSJJU2V5')),
('Еремина', 'Олеся', 'Сергеевна',3,MD5('E89L7XQ9XO'));

-- SELECT u.name 'Имя', u.surname 'Фамилия', u.`password` 'Пароль', r.name 'Вид пользователя'
-- FROM `users` u
-- JOIN `roles` r ON r.id=u.role_id;

DROP TABLE IF EXISTS `tests`;
CREATE TABLE `tests` (
	`id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	`name` VARCHAR(255) NOT NULL, -- название теста
	`description` VARCHAR(512) NOT NULL, -- описание
	`number_of_questions` TINYINT UNSIGNED NOT NULL DEFAULT 0, -- кол-во вопросов
	`time_created` TIMESTAMP NOT NULL DEFAULT NOW(), -- дата создания теста
	`test_start_datetime` TIMESTAMP NOT NULL, -- дата старта теста
	`test_stop_datetime` TIMESTAMP NOT NULL, -- дата завершения теста
	-- `test_execution_time` TIMESTAMP NOT NULL DEFAULT TIMESTAMP('01:00:00'), -- время на выполнение теста (часы, мин, сек), по умолчанию 1 ч
	-- `test_lifetime` TIMESTAMP NOT NULL DEFAULT INTERVAL 7 DAY, -- время жизни теста, по умолчанию 7 д
	CHECK (`test_start_datetime` < `test_stop_datetime` AND `time_created` <= `test_start_datetime` AND `time_created` < `test_stop_datetime`)
);



DROP TABLE IF EXISTS `questions`;
CREATE TABLE `questions` (
	`id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	`test_id` BIGINT NOT NULL, -- id test'а
	`text` VARCHAR(255) NOT NULL, -- текст вопроса
	`number_of_answers` TINYINT UNSIGNED NOT NULL DEFAULT 0, -- кол-во ответов. TINYINT UNSIGNED = 0..255 - оптимальный вариант
	`multiple_answer` BOOL NOT NULL DEFAULT FALSE, -- множество ответов
	FOREIGN KEY (test_id) REFERENCES tests(id)
);

DROP TABLE IF EXISTS `answers`;
CREATE TABLE `answers` (
	`id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	`question_id` BIGINT NOT NULL, -- id вопроса
	`text` VARCHAR(255) NOT NULL, -- текст ответа
	`is_true_answer` BOOL NOT NULL DEFAULT FALSE, -- верный ответ
	FOREIGN KEY (question_id) REFERENCES questions(id)
);

-- DROP FUNCTION IF EXISTS add_days;
-- CREATE FUNCTION add_days(`timestamp` TIMESTAMP, `days` INT)
-- RETURNS TIMESTAMP
-- DETERMINISTIC
-- COMMENT 'Return `timestamp` + `days` days'
-- RETURN DATE_ADD(`timestamp`, INTERVAL `days` DAY);

-- DROP FUNCTION IF EXISTS rand_int;
-- CREATE FUNCTION rand_int(`min` INT, `max` INT)
-- RETURNS INT
-- DETERMINISTIC
-- COMMENT 'Gets a random integer between `min` and `max`, bounds included'
-- RETURN FLOOR(`min` + RAND() * (`max` - `min` + 1));

-- SET @test_availability_time = MAKETIME(1, 0, 0); -- время доступности теста (часы, мин, сек) для генерации данных
-- SET @now = NOW();
-- SET @test_start_datetime = DATE_ADD(@now, INTERVAL rand_int(60, 120) MINUTE); -- начало теста через 60-120 минут
-- SET @test_stop_datetime = DATE_ADD(@test_start_datetime, INTERVAL 7 DAY); -- конец теста через 7 дней
-- SELECT @now, @test_start_datetime, @test_stop_datetime, @test_availability_time;



INSERT INTO `tests` (name, description, test_start_datetime, test_stop_datetime)
VALUES
-- /* Присваивание переменной @test_start_datetime для вычисления нового значения @test_stop_datetime (для каждой строки) */
-- ('Тест №1', 'Просто тест', @test_start_datetime := NOW(), add_days(@test_start_datetime,7)),
-- ('Тест №2', 'Обычный тест', @test_start_datetime := NOW(), add_days(@test_start_datetime,10)),
-- ('Тест №3', 'Итоговый тест', @test_start_datetime := NOW(), add_days(@test_start_datetime,14));
('Тест №1', 'Просто тест', NOW(), NOW() + INTERVAL 7 DAY), -- тоже самое что и DATE_ADD(NOW(), INTERVAL 7 DAY)
('Тест №2', 'Обычный тест', NOW(), NOW() + INTERVAL 10 DAY),
('Тест №3', 'Итоговый тест', NOW(), NOW() + INTERVAL 14 DAY);

INSERT INTO `questions`(test_id, `text`)
VALUES
(1, 'Сколько будет 1 + 1?'),
(1, 'Сколько будет 2 + 2?'),
(1, 'Сколько будет 3 + 3?'),
(1, 'Сколько будет 4 + 4?'),
(1, 'Сколько будет 5 + 5?');


SELECT *
FROM tests t
JOIN questions q ON q.test_id = t.id ;

-- SELECT *
-- FROM `tests` t

DROP TABLE IF EXISTS `user_tests`;
CREATE TABLE `user_tests` (
	`id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	`user_id` BIGINT NOT NULL, -- id user'а
	`test_id` BIGINT NOT NULL, -- id test'а
	FOREIGN KEY (user_id) REFERENCES users(id),
	FOREIGN KEY (test_id) REFERENCES tests(id)
);


INSERT INTO `user_tests` (user_id, test_id)
VALUES
(1, 1),
(1, 2),
(1, 3),
(2, 1),
(2, 1),
(2, 1),
(2, 2),
(2, 3),
(3, 1),
(3, 2),
(3, 3),
(4, 1),
(4, 2),
(4, 3),
(5, 1),
(5, 2),
(5, 3),
(6, 1),
(6, 2),
(6, 3),
(7, 1),
(7, 2),
(7, 3),
(8, 1),
(8, 2),
(8, 3),
(9, 1),
(9, 2),
(9, 3),
(10, 1),
(10, 2),
(10, 3),
(11, 1),
(11, 2),
(11, 3);

-- SELECT ut.id 'id теста', u.name 'Имя', t.name 'Тест', t.test_stop_datetime 'Время окончания теста'
-- FROM `user_tests` ut
-- JOIN users u ON u.id = ut.user_id
-- JOIN tests t ON t.id = ut.test_id 
-- ORDER BY ut.id;

-- Кол-во пройденных тестов у каждого пользователя
-- SELECT u.id 'id пользователя', u.name 'Пользователь', count(*) 'Кол-во пройденных тестов'
-- FROM `user_tests` ut
-- JOIN users u ON u.id = ut.user_id
-- JOIN tests t ON t.id = ut.test_id
-- GROUP BY u.id, u.name
-- ORDER BY 'Кол-во пройденных тестов';



-- SELECT UNIX_TIMESTAMP('2015-01-15 12:00:00')
-- SELECT UNIX_TIMESTAMP('12:00:00')

