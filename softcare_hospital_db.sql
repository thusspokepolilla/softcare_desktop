-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 21, 2025 at 04:40 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `softcare_hospital_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `appointments`
--

CREATE TABLE `appointments` (
  `appointment_id` int(11) NOT NULL,
  `doctor_id` int(11) DEFAULT NULL,
  `last_name` varchar(30) NOT NULL,
  `first_name` varchar(30) NOT NULL,
  `middle_name` varchar(30) NOT NULL,
  `appointment_date` date NOT NULL,
  `appointment_time` time NOT NULL,
  `status` varchar(8) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `appointments`
--

INSERT INTO `appointments` (`appointment_id`, `doctor_id`, `last_name`, `first_name`, `middle_name`, `appointment_date`, `appointment_time`, `status`) VALUES
(1, 2, 'Tim', 'Murda', 'Vic', '2025-03-24', '10:00:00', 'Done'),
(6, 8, 'Win', 'Nah', 'Id', '2025-03-31', '09:00:00', 'Pending'),
(7, 8, 'Lose', 'Would', 'You', '2025-04-01', '12:59:00', 'Pending'),
(8, 8, 'Na', 'Pasensya', 'Ka', '2025-04-06', '06:30:00', 'Done');

-- --------------------------------------------------------

--
-- Table structure for table `billings`
--

CREATE TABLE `billings` (
  `billing_id` int(11) NOT NULL,
  `patient_record_id` int(11) DEFAULT NULL,
  `appointment_id` int(11) DEFAULT NULL,
  `mode_of_payment` varchar(11) NOT NULL,
  `total_amount` int(11) NOT NULL,
  `is_paid` varchar(3) NOT NULL,
  `balance` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `billings`
--

INSERT INTO `billings` (`billing_id`, `patient_record_id`, `appointment_id`, `mode_of_payment`, `total_amount`, `is_paid`, `balance`) VALUES
(9, 1, NULL, 'Cash', 15151, 'No', 15151),
(17, 4, NULL, 'Cash', 5000, 'No', 5000),
(18, 5, NULL, 'Credit Card', 99999, 'Yes', 0),
(19, NULL, 6, 'Cash', 5000, 'No', 5000);

-- --------------------------------------------------------

--
-- Table structure for table `patient_records`
--

CREATE TABLE `patient_records` (
  `patient_id` int(11) NOT NULL,
  `doctor_id` int(11) DEFAULT NULL,
  `last_name` varchar(30) NOT NULL,
  `first_name` varchar(30) NOT NULL,
  `middle_name` varchar(30) NOT NULL,
  `age` int(11) NOT NULL,
  `sex` varchar(6) NOT NULL,
  `admission_date` date NOT NULL,
  `admission_time` time NOT NULL,
  `status` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `patient_records`
--

INSERT INTO `patient_records` (`patient_id`, `doctor_id`, `last_name`, `first_name`, `middle_name`, `age`, `sex`, `admission_date`, `admission_time`, `status`) VALUES
(1, 2, 'White', 'Walter', 'Hartwell', 51, 'Male', '2025-03-24', '16:30:00', 'Referred to Other Hospital'),
(3, 3, 'Duterte', 'Rodrigo', 'Roa', 79, 'Male', '2025-03-11', '09:00:00', 'Referred to Other Hospital'),
(4, 8, 'User', 'Testing', '', 99, 'Female', '2025-04-01', '23:59:00', 'Admitted'),
(5, 8, 'Gojo', 'Satoru', 'Fraud', 26, 'Male', '2025-03-05', '11:59:00', 'Discharged');

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `user_id` int(11) NOT NULL,
  `username` varchar(30) NOT NULL,
  `password` char(60) NOT NULL,
  `role` varchar(6) NOT NULL,
  `last_name` varchar(30) NOT NULL,
  `first_name` varchar(30) NOT NULL,
  `middle_name` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`user_id`, `username`, `password`, `role`, `last_name`, `first_name`, `middle_name`) VALUES
(1, 'ingalclarence2004', '$2a$13$Z2oB.egMA1rypVTlh3UbxegoOPobu8Cd6uWJV5hE1l0qfvQztRSEO', 'Admin', 'Ingal', 'Clarence', 'Ventura'),
(2, 'housemd', '$2a$13$BOucndlVqlBf6.UryZF4GOqY9Jmdk.Qu8lHBei/QZQLmdt44/bMAC', 'Doctor', 'House', 'Gregory', ''),
(3, 'drstrange', '$2a$13$bJH4lPMo/vFQGqlMkAGvou31DmRLB3chqdcP1bxHpe9XMr7nvuale', 'Doctor', 'Strange', 'Stephen', ''),
(4, 'ardentapollo06', '$2a$13$FDV15Awz5SRxeBuXkPh7pe5DAfrmRm8GKtfw2CuJBlulXfEuAVAB2', 'Admin', 'Sta. Ana', 'Arlan Gabriel', 'Ingal'),
(8, 'drratio', '$2a$13$sy4WHGVE7n13eKxHdNaZnOiz2GZvN6J4crrCpNYqv/IlBoD/7xPPS', 'Doctor', 'Ratio', 'Veritas', '');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `appointments`
--
ALTER TABLE `appointments`
  ADD PRIMARY KEY (`appointment_id`),
  ADD KEY `doctor_id` (`doctor_id`);

--
-- Indexes for table `billings`
--
ALTER TABLE `billings`
  ADD PRIMARY KEY (`billing_id`),
  ADD UNIQUE KEY `patient_record_id` (`patient_record_id`) USING BTREE,
  ADD UNIQUE KEY `appointment_id` (`appointment_id`) USING BTREE;

--
-- Indexes for table `patient_records`
--
ALTER TABLE `patient_records`
  ADD PRIMARY KEY (`patient_id`),
  ADD KEY `doctor_id` (`doctor_id`) USING BTREE;

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`user_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `appointments`
--
ALTER TABLE `appointments`
  MODIFY `appointment_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `billings`
--
ALTER TABLE `billings`
  MODIFY `billing_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- AUTO_INCREMENT for table `patient_records`
--
ALTER TABLE `patient_records`
  MODIFY `patient_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `appointments`
--
ALTER TABLE `appointments`
  ADD CONSTRAINT `appointment_doctor` FOREIGN KEY (`doctor_id`) REFERENCES `users` (`user_id`) ON DELETE SET NULL ON UPDATE CASCADE;

--
-- Constraints for table `billings`
--
ALTER TABLE `billings`
  ADD CONSTRAINT `appointment_billing` FOREIGN KEY (`appointment_id`) REFERENCES `appointments` (`appointment_id`) ON DELETE SET NULL ON UPDATE CASCADE,
  ADD CONSTRAINT `patient_billing` FOREIGN KEY (`patient_record_id`) REFERENCES `patient_records` (`patient_id`) ON DELETE SET NULL ON UPDATE CASCADE;

--
-- Constraints for table `patient_records`
--
ALTER TABLE `patient_records`
  ADD CONSTRAINT `patient_doctor` FOREIGN KEY (`doctor_id`) REFERENCES `users` (`user_id`) ON DELETE SET NULL ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
