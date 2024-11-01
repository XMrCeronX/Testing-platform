-- Скрипт на создание базы данных "Платформа для тестирования"

USE `sys`;

DROP DATABASE IF EXISTS `test_platform`;
CREATE DATABASE `test_platform`;

USE `test_platform`;

DROP TABLE IF EXISTS `roles`;
CREATE TABLE `roles` (
	`id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	`name` VARCHAR(255) NOT NULL -- имя роли
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
	-- Return a random number >= 5 and <=10: UNIX_TIMESTAMP(DATE_ADD(NOW(), INTERVAL FLOOR(RAND()*(10-2+1)+2) MINUTE))
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

SELECT u.name 'Имя', u.surname 'Фамилия', u.`password` 'Пароль', r.name 'Вид пользователя'
FROM `users` u
JOIN `roles` r ON r.id=u.role_id;


-- SELECT UNIX_TIMESTAMP(DATE_ADD(NOW(), INTERVAL FLOOR(RAND()*(10-2+1)+2) MINUTE));

