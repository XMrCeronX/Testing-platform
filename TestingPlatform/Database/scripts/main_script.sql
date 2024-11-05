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
	`login` VARCHAR(255) NOT NULL UNIQUE, -- логин
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

INSERT INTO `users` (patronymic, name, surname, role_id, `login`, `password`)
VALUES
('гуляй вася', 'root', 'Не твоего ума дело',1,'root', MD5('password')),
('Симонова', 'Ксения', 'Ивановна',2,'catixa-xuta98@bk.ru', MD5('OWJ895OVXV')),
('Старостина', 'Елизавета', 'Фёдоровна',2, 'kuyano-wimi6@yandex.ru', MD5('I9F3DNHPWL')),
('Васильев', 'Святослав', 'Михайлович',2, 'pujef_ikene11@hotmail.com', MD5('ISUWSC7KI3')),
('Лазарева', 'Варвара', 'Кирилловна',2, 'bunane-siwe91@hotmail.com', MD5('GAMZCLFZGX')),
('Тарасов', 'Даниил', 'Никитич',2, 'zebuj-anoje53@list.ru', MD5('MBUJ90D2TE')),
('Сафонов', 'Алексей', 'Никитич',3, 'poved_uroge90@internet.ru', MD5('0772AYSGLV')),
('Филимонов', 'Данила', 'Даниилович',3, 'var-ixafata88@internet.ru', MD5('C0S3OV69BW')),
('Фомичева', 'Айлин', 'Александровна',3, 'yik-alepuso51@inbox.ru', MD5('U8GIOPNR42')),
('Николаева', 'Виктория', 'Дмитриевна',3, 'gijudol-ora21@yahoo.com', MD5('DUCSJJU2V5')),
('Еремина', 'Олеся', 'Сергеевна',3, 'fopata_vere35@list.ru', MD5('E89L7XQ9XO'));

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

delimiter |
CREATE TRIGGER trigger_update_number_of_questions_in_test_before_inserting
BEFORE INSERT ON `questions`
FOR EACH ROW
BEGIN
	SET @CONST_MAX_NUMBER_OF_QUESTIONS = 255;
	SET @question_count = (SELECT COUNT(*) FROM questions q WHERE q.test_id = NEW.test_id);
	IF @question_count >= @CONST_MAX_NUMBER_OF_QUESTIONS THEN
    	SET @message = concat('MaximumNumberOfQuestionsTriggerError: Попытка вставки нового вопроса превышает максимально допустимое значение = ', cast(@CONST_MAX_NUMBER_OF_QUESTIONS as char), '.');
        signal sqlstate '45000' SET message_text = @message;
    ELSE
		UPDATE tests t
		SET t.number_of_questions = @question_count + 1
		WHERE t.id = NEW.test_id;
    END IF;
END;|
delimiter ;

delimiter |
CREATE TRIGGER trigger_update_number_of_questions_in_test_before_deleting
BEFORE DELETE ON `questions`
FOR EACH ROW
BEGIN
	UPDATE tests t
	SET t.number_of_questions = t.number_of_questions - 1
	WHERE t.id = OLD.test_id;
END;|
delimiter ;

-- надо добавить для целостности данных
-- CREATE TRIGGER ***
-- BEFORE UPDATE ON `questions`

DROP TABLE IF EXISTS `answers`;
CREATE TABLE `answers` (
	`id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	`question_id` BIGINT NOT NULL, -- id вопроса
	`text` VARCHAR(255) NOT NULL, -- текст ответа
	`is_true_answer` BOOL NOT NULL DEFAULT FALSE, -- верный ответ
	FOREIGN KEY (question_id) REFERENCES questions(id)
);

delimiter |
CREATE TRIGGER trigger_update_number_of_answers_in_question_before_inserting
BEFORE INSERT ON `answers`
FOR EACH ROW
BEGIN
	SET @CONST_MAX_NUMBER_OF_ANSWERS = 255;
	SET @answer_count = (SELECT COUNT(*) FROM answers a WHERE a.question_id = NEW.question_id);
	IF @answer_count >= @CONST_MAX_NUMBER_OF_ANSWERS THEN
    	SET @message = concat('MaximumNumberOfAnswersTriggerError: Попытка вставки нового ответа превышает максимально допустимое значение = ', cast(@CONST_MAX_NUMBER_OF_ANSWERS as char), '.');
        signal sqlstate '45000' SET message_text = @message;
    ELSE
		UPDATE questions q
		SET q.number_of_answers = @answer_count + 1
		WHERE q.id = NEW.question_id;
    END IF;
END;|
delimiter ;

delimiter |
CREATE TRIGGER trigger_update_number_of_answers_in_question_before_deleting
BEFORE DELETE ON `answers`
FOR EACH ROW
BEGIN
	UPDATE questions q
	SET q.number_of_answers = q.number_of_answers - 1
	WHERE q.id = OLD.question_id;
END;|
delimiter ;

-- надо добавить для целостности данных
-- CREATE TRIGGER ***
-- BEFORE UPDATE ON `answers`

-- Тирггеры на обработку multiple_answer:
delimiter |
CREATE TRIGGER trigger_update_multiple_answer_in_question_before_inserting
BEFORE INSERT ON `answers`
FOR EACH ROW
BEGIN
	IF NEW.is_true_answer THEN -- если новый ответ верный
		SET @true_answer_count = (SELECT COUNT(*) FROM answers a WHERE a.question_id = NEW.question_id AND a.is_true_answer);
		IF @true_answer_count = 1 THEN -- уже есть верный ответ у вопроса --> надо поменять multiple_answer = TRUE
			-- не использую "@true_answer_count > 1" 
			-- т.к. обновляться multiple_answer будет каждую новую вставку 
			-- т.е. меняю раз при двух верных ответах (уже существующий: true_answer_count + новая запись с верным ответом: NEW)
			-- получается маленькая {большая при большом кол-ве запросов;)} оптимизация
			UPDATE questions q
			SET multiple_answer = TRUE
			WHERE q.id = NEW.question_id;
		END IF;
	END IF;
END;|
delimiter ;

delimiter |
CREATE TRIGGER trigger_update_multiple_answer_in_question_before_deleting
BEFORE DELETE ON `answers`
FOR EACH ROW
BEGIN
	IF OLD.is_true_answer THEN -- если удаляемый ответ верный
		SET @true_answer_count = (SELECT COUNT(*) FROM answers a WHERE a.question_id = OLD.question_id AND a.is_true_answer);
		IF @true_answer_count = 2 THEN -- удаляемый будет удален --> останется 1 верный ответ
			UPDATE questions q
			SET multiple_answer = FALSE
			WHERE q.id = OLD.question_id;
		END IF;
	END IF;
END;|
delimiter ;

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
(2, '1 + 1 = 3?');

-- DELETE FROM questions
-- WHERE id BETWEEN 4 AND 6;

-- SELECT t.name 'имя теста', t.number_of_questions 'Кол-во вопросов'
-- FROM tests t
-- JOIN questions q ON q.test_id = t.id
-- GROUP BY t.id;

INSERT INTO answers (question_id, `text`, is_true_answer)
VALUES
(1, '1', FALSE),
(1, '2', TRUE),
(1, '3', FALSE),
(1, '4', FALSE),
(2, '4', TRUE),
(2, '7', FALSE),
(2, '9', FALSE),
(3, '8 + 1', TRUE),
(3, '7 + 2', TRUE),
(3, '9', TRUE),
(3, '10 - 2 + 1', TRUE),
(3, '11', FALSE),
(3, '120', FALSE),
(4,'да',TRUE),
(4,'нет',FALSE);

-- INSERT INTO answers (question_id, `text`, is_true_answer)
-- VALUES
-- (4,'2',FALSE),
-- (4,'3',FALSE),
-- (4,'4',FALSE),
-- (4,'5',FALSE),
-- (4,'6',FALSE),
-- (4,'7',FALSE),
-- (4,'8',FALSE),
-- (4,'9',FALSE),
-- (4,'10',FALSE),
-- (4,'11',FALSE),
-- (4,'12',FALSE),
-- (4,'13',FALSE),
-- (4,'14',FALSE),
-- (4,'15',FALSE),
-- (4,'16',FALSE),
-- (4,'17',FALSE),
-- (4,'18',FALSE),
-- (4,'19',FALSE),
-- (4,'20',FALSE),
-- (4,'21',FALSE),
-- (4,'22',FALSE),
-- (4,'23',FALSE),
-- (4,'24',FALSE),
-- (4,'25',FALSE),
-- (4,'26',FALSE),
-- (4,'27',FALSE),
-- (4,'28',FALSE),
-- (4,'29',FALSE),
-- (4,'30',FALSE),
-- (4,'31',FALSE),
-- (4,'32',FALSE),
-- (4,'33',FALSE),
-- (4,'34',FALSE),
-- (4,'35',FALSE),
-- (4,'36',FALSE),
-- (4,'37',FALSE),
-- (4,'38',FALSE),
-- (4,'39',FALSE),
-- (4,'40',FALSE),
-- (4,'41',FALSE),
-- (4,'42',FALSE),
-- (4,'43',FALSE),
-- (4,'44',FALSE),
-- (4,'45',FALSE),
-- (4,'46',FALSE),
-- (4,'47',FALSE),
-- (4,'48',FALSE),
-- (4,'49',FALSE),
-- (4,'50',FALSE),
-- (4,'51',FALSE),
-- (4,'52',FALSE),
-- (4,'53',FALSE),
-- (4,'54',FALSE),
-- (4,'55',FALSE),
-- (4,'56',FALSE),
-- (4,'57',FALSE),
-- (4,'58',FALSE),
-- (4,'59',FALSE),
-- (4,'60',FALSE),
-- (4,'61',FALSE),
-- (4,'62',FALSE),
-- (4,'63',FALSE),
-- (4,'64',FALSE),
-- (4,'65',FALSE),
-- (4,'66',FALSE),
-- (4,'67',FALSE),
-- (4,'68',FALSE),
-- (4,'69',FALSE),
-- (4,'70',FALSE),
-- (4,'71',FALSE),
-- (4,'72',FALSE),
-- (4,'73',FALSE),
-- (4,'74',FALSE),
-- (4,'75',FALSE),
-- (4,'76',FALSE),
-- (4,'77',FALSE),
-- (4,'78',FALSE),
-- (4,'79',FALSE),
-- (4,'80',FALSE),
-- (4,'81',FALSE),
-- (4,'82',FALSE),
-- (4,'83',FALSE),
-- (4,'84',FALSE),
-- (4,'85',FALSE),
-- (4,'86',FALSE),
-- (4,'87',FALSE),
-- (4,'88',FALSE),
-- (4,'89',FALSE),
-- (4,'90',FALSE),
-- (4,'91',FALSE),
-- (4,'92',FALSE),
-- (4,'93',FALSE),
-- (4,'94',FALSE),
-- (4,'95',FALSE),
-- (4,'96',FALSE),
-- (4,'97',FALSE),
-- (4,'98',FALSE),
-- (4,'99',FALSE),
-- (4,'100',FALSE),
-- (4,'101',FALSE),
-- (4,'102',FALSE),
-- (4,'103',FALSE),
-- (4,'104',FALSE),
-- (4,'105',FALSE),
-- (4,'106',FALSE),
-- (4,'107',FALSE),
-- (4,'108',FALSE),
-- (4,'109',FALSE),
-- (4,'110',FALSE),
-- (4,'111',FALSE),
-- (4,'112',FALSE),
-- (4,'113',FALSE),
-- (4,'114',FALSE),
-- (4,'115',FALSE),
-- (4,'116',FALSE),
-- (4,'117',FALSE),
-- (4,'118',FALSE),
-- (4,'119',FALSE),
-- (4,'120',FALSE),
-- (4,'121',FALSE),
-- (4,'122',FALSE),
-- (4,'123',FALSE),
-- (4,'124',FALSE),
-- (4,'125',FALSE),
-- (4,'126',FALSE),
-- (4,'127',FALSE),
-- (4,'128',FALSE),
-- (4,'129',FALSE),
-- (4,'130',FALSE),
-- (4,'131',FALSE),
-- (4,'132',FALSE),
-- (4,'133',FALSE),
-- (4,'134',FALSE),
-- (4,'135',FALSE),
-- (4,'136',FALSE),
-- (4,'137',FALSE),
-- (4,'138',FALSE),
-- (4,'139',FALSE),
-- (4,'140',FALSE),
-- (4,'141',FALSE),
-- (4,'142',FALSE),
-- (4,'143',FALSE),
-- (4,'144',FALSE),
-- (4,'145',FALSE),
-- (4,'146',FALSE),
-- (4,'147',FALSE),
-- (4,'148',FALSE),
-- (4,'149',FALSE),
-- (4,'150',FALSE),
-- (4,'151',FALSE),
-- (4,'152',FALSE),
-- (4,'153',FALSE),
-- (4,'154',FALSE),
-- (4,'155',FALSE),
-- (4,'156',FALSE),
-- (4,'157',FALSE),
-- (4,'158',FALSE),
-- (4,'159',FALSE),
-- (4,'160',FALSE),
-- (4,'161',FALSE),
-- (4,'162',FALSE),
-- (4,'163',FALSE),
-- (4,'164',FALSE),
-- (4,'165',FALSE),
-- (4,'166',FALSE),
-- (4,'167',FALSE),
-- (4,'168',FALSE),
-- (4,'169',FALSE),
-- (4,'170',FALSE),
-- (4,'171',FALSE),
-- (4,'172',FALSE),
-- (4,'173',FALSE),
-- (4,'174',FALSE),
-- (4,'175',FALSE),
-- (4,'176',FALSE),
-- (4,'177',FALSE),
-- (4,'178',FALSE),
-- (4,'179',FALSE),
-- (4,'180',FALSE),
-- (4,'181',FALSE),
-- (4,'182',FALSE),
-- (4,'183',FALSE),
-- (4,'184',FALSE),
-- (4,'185',FALSE),
-- (4,'186',FALSE),
-- (4,'187',FALSE),
-- (4,'188',FALSE),
-- (4,'189',FALSE),
-- (4,'190',FALSE),
-- (4,'191',FALSE),
-- (4,'192',FALSE),
-- (4,'193',FALSE),
-- (4,'194',FALSE),
-- (4,'195',FALSE),
-- (4,'196',FALSE),
-- (4,'197',FALSE),
-- (4,'198',FALSE),
-- (4,'199',FALSE),
-- (4,'200',FALSE),
-- (4,'201',FALSE),
-- (4,'202',FALSE),
-- (4,'203',FALSE),
-- (4,'204',FALSE),
-- (4,'205',FALSE),
-- (4,'206',FALSE),
-- (4,'207',FALSE),
-- (4,'208',FALSE),
-- (4,'209',FALSE),
-- (4,'210',FALSE),
-- (4,'211',FALSE),
-- (4,'212',FALSE),
-- (4,'213',FALSE),
-- (4,'214',FALSE),
-- (4,'215',FALSE),
-- (4,'216',FALSE),
-- (4,'217',FALSE),
-- (4,'218',FALSE),
-- (4,'219',FALSE),
-- (4,'220',FALSE),
-- (4,'221',FALSE),
-- (4,'222',FALSE),
-- (4,'223',FALSE),
-- (4,'224',FALSE),
-- (4,'225',FALSE),
-- (4,'226',FALSE),
-- (4,'227',FALSE),
-- (4,'228',FALSE),
-- (4,'229',FALSE),
-- (4,'230',FALSE),
-- (4,'231',FALSE),
-- (4,'232',FALSE),
-- (4,'233',FALSE),
-- (4,'234',FALSE),
-- (4,'235',FALSE),
-- (4,'236',FALSE),
-- (4,'237',FALSE),
-- (4,'238',FALSE),
-- (4,'239',FALSE),
-- (4,'240',FALSE),
-- (4,'241',FALSE),
-- (4,'242',FALSE),
-- (4,'243',FALSE),
-- (4,'244',FALSE),
-- (4,'245',FALSE),
-- (4,'246',FALSE),
-- (4,'247',FALSE),
-- (4,'248',FALSE),
-- (4,'249',FALSE),
-- (4,'250',FALSE),
-- (4,'251',FALSE),
-- (4,'252',FALSE),
-- (4,'253',FALSE),
-- (4,'254',FALSE); -- если добавить (4,'255',FALSE) тогда получим исключение

-- SELECT 
-- 	q.id 'id вопроса',
-- 	q.`text` 'Вопрос',
-- 	a.`text` 'Ответ',
-- 	CASE 
-- 		WHEN a.is_true_answer THEN '+' -- если != 0 (=> 1/true)
-- 		ELSE ''
-- 	END 'Ответ верный',
-- 	CASE 
-- 		WHEN q.multiple_answer THEN CONCAT('Много (',(SELECT COUNT(*) FROM answers ans WHERE ans.question_id = a.question_id AND ans.is_true_answer),')') -- если != 0 (=> 1/true)
-- 		ELSE 'Один'
-- 	END 'Кол-во ответов'
-- FROM questions q 
-- JOIN answers a ON a.question_id = q.id;

-- DELETE FROM answers a
-- WHERE a.id BETWEEN 9 AND 11;
-- 
-- INSERT INTO answers (question_id, `text`, is_true_answer)
-- VALUES (3, 'Новый верный текст', TRUE);

DROP TABLE IF EXISTS `user_tests`; -- промежуточная таблица, которая делает связь "many to many" между users и tests
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

-- Кол-во тестов у каждого пользователя
-- SELECT u.id 'id пользователя', u.name 'Пользователь', count(*) 'Кол-во тестов'
-- FROM `user_tests` ut
-- JOIN users u ON u.id = ut.user_id
-- JOIN tests t ON t.id = ut.test_id
-- GROUP BY u.id
-- ORDER BY u.id;

-- SELECT UNIX_TIMESTAMP('2015-01-15 12:00:00')
-- SELECT UNIX_TIMESTAMP('12:00:00')
