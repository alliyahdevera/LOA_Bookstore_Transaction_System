-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: Oct 04, 2026 at 12:51 PM
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
-- Database: `loa_bookstore`
--

-- --------------------------------------------------------

--
-- Table structure for table `tbl_audit_logs`
--

CREATE TABLE `tbl_audit_logs` (
  `audit_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `log_type` enum('Activity','Price Change','Login') NOT NULL DEFAULT 'Activity',
  `action_type` varchar(100) NOT NULL,
  `reference_no` varchar(100) DEFAULT NULL,
  `product_code` varchar(50) DEFAULT NULL,
  `product_name` varchar(150) DEFAULT NULL,
  `old_price` decimal(10,2) DEFAULT NULL,
  `new_price` decimal(10,2) DEFAULT NULL,
  `details` text DEFAULT NULL,
  `status` varchar(50) DEFAULT NULL,
  `reason` text DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_audit_logs`
--

INSERT INTO `tbl_audit_logs` (`audit_id`, `user_id`, `log_type`, `action_type`, `reference_no`, `product_code`, `product_name`, `old_price`, `new_price`, `details`, `status`, `reason`, `created_at`) VALUES
(1, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 15:10:26'),
(2, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 15:18:06'),
(3, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 15:19:38'),
(4, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 15:22:41'),
(5, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 15:24:21'),
(6, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 15:25:15'),
(7, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 15:26:01'),
(8, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 15:27:23'),
(9, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 15:31:43'),
(10, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 15:41:01'),
(11, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 16:45:58'),
(12, 1, 'Activity', 'Update Student', '2435-23', NULL, NULL, NULL, NULL, 'Updated Lucas Castillo', 'Success', NULL, '2026-10-01 16:52:02'),
(13, 1, 'Activity', 'Update Student', '1235-23', NULL, NULL, NULL, NULL, 'Updated Sofia Aquino', 'Success', NULL, '2026-10-01 16:52:15'),
(14, 1, 'Activity', 'Update Student', '2235-20', NULL, NULL, NULL, NULL, 'Updated Miguel Dela Cruz', 'Success', NULL, '2026-10-01 16:52:30'),
(15, 1, 'Activity', 'Update Student', '1785-23', NULL, NULL, NULL, NULL, 'Updated Patricia Domingo', 'Success', NULL, '2026-10-01 16:52:38'),
(16, 1, 'Activity', 'Update Student', '2343-22', NULL, NULL, NULL, NULL, 'Updated Carlo Fernandez', 'Success', NULL, '2026-10-01 16:53:11'),
(17, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 18:10:14'),
(18, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 18:14:40'),
(19, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 20:19:56'),
(20, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 20:20:20'),
(21, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 20:22:31'),
(22, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 20:23:56'),
(23, 8, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 20:25:23'),
(24, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 21:06:06'),
(25, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'Incorrect password entered', 'Failed - Incorrect Password', NULL, '2026-10-01 21:12:29'),
(26, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 21:12:32'),
(27, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'Incorrect password entered', 'Failed - Incorrect Password', NULL, '2026-10-01 21:14:14'),
(28, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-01 21:14:17'),
(29, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 11:31:43'),
(30, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'Incorrect password entered', 'Failed - Incorrect Password', NULL, '2026-10-02 18:22:41'),
(31, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 18:22:44'),
(32, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 18:25:31'),
(33, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 18:27:20'),
(34, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 18:43:22'),
(35, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 20:33:20'),
(36, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 20:51:35'),
(37, 1, 'Activity', 'Sale', 'TXN-20261002205141910', NULL, NULL, NULL, NULL, 'Sale to Alliyah De Vera - Total: 25.00 (Cash, OR OR-20261002205150475)', 'Success', NULL, '2026-10-02 20:54:02'),
(38, 1, 'Activity', 'Sale', 'TXN-20261002205439014', NULL, NULL, NULL, NULL, 'Sale to 123 - Total: 1,225.00 (Cash, OR OR-20261002205507126)', 'Success', NULL, '2026-10-02 20:55:14'),
(39, 1, 'Activity', 'Stock In', 'DR-20261002210016648', NULL, NULL, NULL, NULL, 'Received 1 pc(s) of SUP-ART - Art Paper', 'Success', NULL, '2026-10-02 21:00:20'),
(40, 1, 'Activity', 'Inventory Count', 'CNT-20261002210232', NULL, NULL, NULL, NULL, 'Saved 1 counted item(s), 1 discrepancy(ies)', 'Success', NULL, '2026-10-02 21:02:32'),
(41, 1, 'Activity', 'Inventory Adjustment', 'CNT-20261002210232', NULL, NULL, NULL, NULL, 'SUP-ART (N/A): 101 -> 10. Reason: e', 'Success', NULL, '2026-10-02 21:02:59'),
(42, 1, 'Activity', 'Item Return', 'RET-20261002210533009', NULL, NULL, NULL, NULL, 'Return of 4 item(s) from TXN-20261002205439014. Reason: dsa', 'Success', NULL, '2026-10-02 21:05:33'),
(43, 6, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 21:09:20'),
(44, 6, 'Activity', 'Sale', 'TXN-20261002210939244', NULL, NULL, NULL, NULL, 'Sale to Miguel Dela Cruz - Total: 25.00 (Salary Deduction, OR OR-20261002211016756)', 'Success', NULL, '2026-10-02 21:10:27'),
(45, 6, 'Activity', 'End of Day', 'EOD-20261002211039', NULL, NULL, NULL, NULL, 'Remittance REM-20261002211107095 - Expected 25.00, Actual 25.00 (Balanced)', 'Success', NULL, '2026-10-02 21:11:07'),
(46, 5, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 21:14:00'),
(47, 7, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 21:14:54'),
(48, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 21:16:33'),
(49, 1, 'Activity', 'Update Product', 'SUP-ART', NULL, NULL, NULL, NULL, 'Updated product \'Art Paper\'', 'Success', NULL, '2026-10-02 21:18:19'),
(50, 1, 'Price Change', 'Price Change', NULL, 'SUP-ART', 'Art Paper', 10.00, 12.00, NULL, 'Success', 'Price Update', '2026-10-02 21:18:19'),
(51, 2, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 21:19:31'),
(52, 2, 'Activity', 'Sale', 'TXN-20261002211935813', NULL, NULL, NULL, NULL, 'Sale to Alliyah De Vera - Total: 850.00 (Cash, OR OR-20261002212507211)', 'Success', NULL, '2026-10-02 21:25:25'),
(53, 2, 'Activity', 'Item Exchange', 'EXC-20261002212805372', NULL, NULL, NULL, NULL, 'Exchange of 1 item(s) from TXN-20261002211935813. Reason: wala lng', 'Success', NULL, '2026-10-02 21:28:05'),
(54, 2, 'Activity', 'End of Day', 'EOD-20261002212821', NULL, NULL, NULL, NULL, 'Remittance REM-20261002212859570 - Expected 850.00, Actual 1,100.00 (Over)', 'Success', NULL, '2026-10-02 21:28:59'),
(55, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'Incorrect password entered', 'Failed - Incorrect Password', NULL, '2026-10-02 21:29:26'),
(56, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 21:29:29'),
(57, 1, 'Activity', 'Inventory Count', 'CNT-20261002213112', NULL, NULL, NULL, NULL, 'Saved 2 counted item(s), 2 discrepancy(ies)', 'Success', NULL, '2026-10-02 21:31:13'),
(58, 1, 'Activity', 'Inventory Adjustment', 'CNT-20261002213112', NULL, NULL, NULL, NULL, 'SUP-ART (N/A): 10 -> 5. Reason: 231231', 'Success', NULL, '2026-10-02 21:31:54'),
(59, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 22:03:31'),
(60, 1, 'Activity', 'End of Day', 'EOD-20261002220354', NULL, NULL, NULL, NULL, 'Remittance REM-20261002220442506 - Expected 1,250.00, Actual 1,250.00 (Balanced)', 'Success', NULL, '2026-10-02 22:04:42'),
(61, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 22:33:03'),
(62, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 22:35:35'),
(63, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 22:35:48'),
(64, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 22:57:47'),
(65, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 23:10:29'),
(66, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 23:12:13'),
(67, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 23:12:27'),
(68, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 23:14:42'),
(69, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 23:16:15'),
(70, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-02 23:16:32'),
(71, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-03 12:33:00'),
(72, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-03 12:38:29'),
(73, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-03 12:41:21'),
(74, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-03 12:58:50'),
(75, 2, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-03 13:00:35'),
(76, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-04 13:07:24'),
(77, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-04 13:21:57'),
(78, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-04 13:36:58'),
(79, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-04 13:45:39'),
(80, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-04 13:50:23'),
(81, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-04 14:36:30'),
(82, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-04 15:00:23'),
(83, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-04 15:04:47'),
(84, 1, 'Activity', 'Update Student', '1235-23', NULL, NULL, NULL, NULL, 'Updated Sofia Aquino', 'Success', NULL, '2026-10-04 15:11:38'),
(85, 1, 'Activity', 'Add Student', '2657-24', NULL, NULL, NULL, NULL, 'Added Alyah Mikhailovna', 'Success', NULL, '2026-10-04 15:12:21'),
(86, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-04 15:21:02'),
(87, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-04 15:25:49'),
(88, 1, 'Activity', 'Item Exchange', 'EXC-20261004153729154', NULL, NULL, NULL, NULL, 'Exchange of 1 item(s) from TXN-20261004152552471. Reason: 12321', 'Success', NULL, '2026-10-04 15:37:29'),
(89, 1, 'Activity', 'Update Student', '2324-24', NULL, NULL, NULL, NULL, 'Updated Angela Mercado', 'Success', NULL, '2026-10-04 15:45:11'),
(90, 1, 'Activity', 'Update Student', '2864-23', NULL, NULL, NULL, NULL, 'Updated Joshua Navarro', 'Success', NULL, '2026-10-04 15:45:20'),
(91, 1, 'Activity', 'Update Student', '1234-22', NULL, NULL, NULL, NULL, 'Updated Bianca Pascual', 'Success', NULL, '2026-10-04 15:45:32'),
(92, 1, 'Activity', 'Update Student', '1543-23', NULL, NULL, NULL, NULL, 'Updated Gabriel Ramirez', 'Success', NULL, '2026-10-04 15:45:52'),
(93, 1, 'Activity', 'Update Student', '2346-24', NULL, NULL, NULL, NULL, 'Updated Isabella Villanueva', 'Success', NULL, '2026-10-04 15:45:58'),
(94, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-04 16:30:52'),
(95, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-04 16:33:34'),
(96, 1, 'Login', 'User Login', NULL, NULL, NULL, NULL, NULL, 'User logged into system', 'Success', NULL, '2026-10-04 16:52:19');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_cash_denominations`
--

CREATE TABLE `tbl_cash_denominations` (
  `denomination_id` int(11) NOT NULL,
  `denomination` decimal(10,2) NOT NULL,
  `quantity` int(11) NOT NULL DEFAULT 0,
  `amount` decimal(10,2) NOT NULL DEFAULT 0.00
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_categories`
--

CREATE TABLE `tbl_categories` (
  `category_id` int(11) NOT NULL,
  `category_name` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_categories`
--

INSERT INTO `tbl_categories` (`category_id`, `category_name`) VALUES
(2, 'Books'),
(3, 'Modules'),
(5, 'Office Supplies'),
(6, 'Other Bookstore Items'),
(4, 'School Supplies'),
(1, 'Uniforms');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_category_types`
--

CREATE TABLE `tbl_category_types` (
  `category_type_id` int(11) NOT NULL,
  `category_id` int(11) NOT NULL,
  `type_name` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_category_types`
--

INSERT INTO `tbl_category_types` (`category_type_id`, `category_id`, `type_name`) VALUES
(1, 1, 'Elementary Uniform'),
(2, 1, 'Junior High Uniform'),
(3, 1, 'Senior High Uniform'),
(4, 1, 'College Uniform'),
(5, 1, 'PE Uniform'),
(6, 2, 'Textbooks'),
(7, 3, 'Learning Modules'),
(8, 4, 'Writing Materials'),
(9, 4, 'Notebooks and Paper'),
(10, 5, 'Filing and Folders'),
(11, 2, 'Grade School Textbooks'),
(12, 2, 'Junior High Textbooks'),
(13, 2, 'Senior High Textbooks'),
(14, 2, 'College Textbooks'),
(15, 3, 'Laboratory Manuals'),
(16, 3, 'Course Packets'),
(17, 4, 'Art & Measuring Tools'),
(18, 5, 'Administrative Forms'),
(19, 5, 'Office Stationery'),
(20, 6, 'Official LOA Merchandise'),
(21, 6, 'Campus Apparel & Accessories'),
(22, 4, 'Paper, Pads & Notebooks'),
(23, 5, 'Clips & Fasteners'),
(24, 5, 'Forms & Record Books'),
(25, 5, 'Packaging & Laminating'),
(26, 5, 'Filing & Document Holders'),
(27, 4, 'Adhesives & Covers'),
(28, 4, 'Art Materials'),
(29, 4, 'Measuring & Math Tools'),
(30, 6, 'Personal & Hygiene Items'),
(31, 4, 'Reference Materials'),
(32, 4, 'Cutting Tools'),
(33, 6, 'Sewing & Craft Accessories'),
(34, 6, 'Student Accessories'),
(35, 1, 'Kinder/Elementary Uniform'),
(36, 1, 'Junior/Senior High Uniform'),
(37, 1, 'College General Uniform'),
(38, 1, 'College Program Uniform'),
(39, 1, 'Special Program Uniform'),
(40, 1, 'Uniform Accessories');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_employees`
--

CREATE TABLE `tbl_employees` (
  `employee_id` int(11) NOT NULL,
  `employee_no` varchar(20) NOT NULL,
  `last_name` varchar(100) NOT NULL,
  `first_name` varchar(100) NOT NULL,
  `department` varchar(100) DEFAULT NULL,
  `job_position` varchar(100) DEFAULT NULL,
  `status` enum('Active','Inactive') NOT NULL DEFAULT 'Active'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_employees`
--

INSERT INTO `tbl_employees` (`employee_id`, `employee_no`, `last_name`, `first_name`, `department`, `job_position`, `status`) VALUES
(1, 'EMP-0001', 'Villanueva', 'Rosa', 'Registrar', 'Registrar Staff', 'Active'),
(2, 'EMP-0002', 'Mercado', 'Daniel', 'Faculty', 'Teacher', 'Active'),
(3, 'EMP-0003', 'Lim', 'Grace', 'Accounting', 'Accounting Staff', 'Active');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_end_of_day`
--

CREATE TABLE `tbl_end_of_day` (
  `end_of_day_id` int(11) NOT NULL,
  `reconciliation_no` varchar(30) NOT NULL,
  `reconciliation_date` date NOT NULL,
  `cashier_id` int(11) NOT NULL,
  `shift` varchar(20) DEFAULT NULL,
  `cash_sales` decimal(10,2) NOT NULL DEFAULT 0.00,
  `salary_deduction` decimal(10,2) NOT NULL DEFAULT 0.00,
  `total_sales` decimal(10,2) NOT NULL DEFAULT 0.00,
  `cash_transaction_count` int(11) NOT NULL DEFAULT 0,
  `salary_deduction_count` int(11) NOT NULL DEFAULT 0,
  `expected_cash` decimal(10,2) NOT NULL DEFAULT 0.00,
  `actual_cash` decimal(10,2) NOT NULL DEFAULT 0.00,
  `difference` decimal(10,2) NOT NULL DEFAULT 0.00,
  `status` enum('Balanced','Short','Over') NOT NULL,
  `remarks` varchar(255) DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_end_of_day`
--

INSERT INTO `tbl_end_of_day` (`end_of_day_id`, `reconciliation_no`, `reconciliation_date`, `cashier_id`, `shift`, `cash_sales`, `salary_deduction`, `total_sales`, `cash_transaction_count`, `salary_deduction_count`, `expected_cash`, `actual_cash`, `difference`, `status`, `remarks`, `created_at`) VALUES
(1, 'EOD-20261002211039', '2026-10-02', 6, 'Night', 25.00, 0.00, 25.00, 1, 0, 25.00, 25.00, 0.00, 'Balanced', 'yes', '2026-10-02 21:11:07'),
(2, 'EOD-20261002212821', '2026-10-02', 2, 'Night', 850.00, 0.00, 850.00, 1, 0, 850.00, 1100.00, 250.00, 'Over', '321', '2026-10-02 21:28:59'),
(3, 'EOD-20261002220354', '2026-10-02', 1, NULL, 1250.00, 0.00, 1250.00, 2, 0, 1250.00, 1250.00, 0.00, 'Balanced', '1231', '2026-10-02 22:04:42');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_inventory_counts`
--

CREATE TABLE `tbl_inventory_counts` (
  `inventory_count_id` int(11) NOT NULL,
  `count_no` varchar(30) NOT NULL,
  `count_date` date NOT NULL,
  `prepared_by` int(11) NOT NULL,
  `status` enum('Pending','Reconciled') NOT NULL DEFAULT 'Pending',
  `remarks` varchar(255) DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_inventory_counts`
--

INSERT INTO `tbl_inventory_counts` (`inventory_count_id`, `count_no`, `count_date`, `prepared_by`, `status`, `remarks`, `created_at`) VALUES
(1, 'CNT-20261002210232', '2026-10-02', 1, 'Reconciled', NULL, '2026-10-02 21:02:32'),
(2, 'CNT-20261002213112', '2026-10-02', 1, 'Pending', NULL, '2026-10-02 21:31:12');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_inventory_count_details`
--

CREATE TABLE `tbl_inventory_count_details` (
  `inventory_count_detail_id` int(11) NOT NULL,
  `inventory_count_id` int(11) NOT NULL,
  `variant_id` int(11) NOT NULL,
  `system_quantity` int(11) NOT NULL,
  `physical_quantity` int(11) NOT NULL,
  `difference` int(11) NOT NULL,
  `status` enum('Matched','Short','Excess') NOT NULL,
  `remarks` varchar(255) DEFAULT NULL,
  `adjusted` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_inventory_count_details`
--

INSERT INTO `tbl_inventory_count_details` (`inventory_count_detail_id`, `inventory_count_id`, `variant_id`, `system_quantity`, `physical_quantity`, `difference`, `status`, `remarks`, `adjusted`) VALUES
(1, 1, 1, 101, 10, -91, 'Short', 'e', 1),
(2, 2, 1, 10, 5, -5, 'Short', '231231', 1),
(3, 2, 746, 30, 35, 5, 'Excess', NULL, 0);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_products`
--

CREATE TABLE `tbl_products` (
  `product_id` int(11) NOT NULL,
  `product_name` varchar(150) NOT NULL,
  `product_description` varchar(255) DEFAULT NULL,
  `source_code` varchar(50) DEFAULT NULL,
  `category_type_id` int(11) NOT NULL,
  `supplier` varchar(150) DEFAULT NULL,
  `unit_cost` decimal(10,2) NOT NULL DEFAULT 0.00,
  `unit_price` decimal(10,2) NOT NULL,
  `status` varchar(20) NOT NULL DEFAULT 'Active'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_products`
--

INSERT INTO `tbl_products` (`product_id`, `product_name`, `product_description`, `source_code`, `category_type_id`, `supplier`, `unit_cost`, `unit_price`, `status`) VALUES
(1, 'Art Paper', '', 'ART', 22, NULL, 0.00, 12.00, 'Active'),
(2, 'Intermediate Pad 1/2 CW', NULL, 'IPC 1/2', 22, NULL, 0.00, 45.00, 'Active'),
(3, 'Binder Clip 1 1/4', NULL, 'BC 1/4', 23, NULL, 0.00, 25.00, 'Active'),
(4, 'Intermediate Pad 1/2 LW', NULL, 'IPL 1/2', 22, NULL, 0.00, 45.00, 'Active'),
(5, 'Binder Clip 1 5/8', NULL, 'BC 5/8', 23, NULL, 0.00, 25.00, 'Active'),
(6, 'Journal Log book', NULL, 'JP', 24, NULL, 0.00, 40.00, 'Active'),
(7, 'Binder Clip 1\"', NULL, 'BC 1', 23, NULL, 0.00, 25.00, 'Active'),
(8, 'Journal Paper', NULL, 'JP', 22, NULL, 0.00, 30.00, 'Active'),
(9, 'Binder Clip 2\"', NULL, 'BC 2', 23, NULL, 0.00, 25.00, 'Active'),
(10, 'Lamination', NULL, 'LAM', 25, NULL, 0.00, 30.00, 'Active'),
(11, 'Bluebooks', NULL, 'BB', 22, NULL, 0.00, 30.00, 'Active'),
(12, 'Ledger Sheet', NULL, 'LS', 24, NULL, 0.00, 40.00, 'Active'),
(13, 'Bond Paper (A4)', NULL, 'BPL', 22, NULL, 0.00, 220.00, 'Active'),
(14, 'Letter Envelope', NULL, 'LE', 26, NULL, 0.00, 15.00, 'Active'),
(15, 'Bond Paper (L)', NULL, 'BPL', 22, NULL, 0.00, 220.00, 'Active'),
(16, 'Manila Paper', NULL, 'MP', 22, NULL, 0.00, 10.00, 'Active'),
(17, 'Bond Paper (S)', NULL, 'BPS', 22, NULL, 0.00, 220.00, 'Active'),
(18, 'Masking Tape 1\"', NULL, NULL, 27, NULL, 0.00, 45.00, 'Active'),
(19, 'Bpen HBW (B)', NULL, 'HBWB', 8, NULL, 0.00, 12.00, 'Active'),
(20, 'Masking Tape 2\"', NULL, NULL, 27, NULL, 0.00, 45.00, 'Active'),
(21, 'Bpen HBW (R)', NULL, 'HBWR', 8, NULL, 0.00, 12.00, 'Active'),
(22, 'Math Notebook', NULL, 'MNB', 22, NULL, 0.00, 35.00, 'Active'),
(23, 'Brown Envelop (L)', NULL, 'BEL', 26, NULL, 0.00, 40.00, 'Active'),
(24, 'Morocco CL Folder (L)', NULL, 'MCL', 26, NULL, 0.00, 20.00, 'Active'),
(25, 'Brown Envelop (S)', NULL, 'BES', 26, NULL, 0.00, 40.00, 'Active'),
(26, 'Morocco CL Folder (S)', NULL, 'MCS', 26, NULL, 0.00, 20.00, 'Active'),
(27, 'Cartolina Assorted', NULL, 'CART', 22, NULL, 0.00, 10.00, 'Active'),
(28, 'Morocco Folder (L)', NULL, 'MOROL', 26, NULL, 0.00, 20.00, 'Active'),
(29, 'Cartolina Black', NULL, 'CARB', 22, NULL, 0.00, 10.00, 'Active'),
(30, 'Morocco Folder (S)', NULL, 'MOROS', 26, NULL, 0.00, 20.00, 'Active'),
(31, 'Cattleya', NULL, 'CAT', 22, NULL, 0.00, 30.00, 'Active'),
(32, 'Notebook (Composition)', NULL, 'NB', 22, NULL, 0.00, 35.00, 'Active'),
(33, 'Clearbook (L)', NULL, 'CBL', 26, NULL, 0.00, 55.00, 'Active'),
(34, 'Notebook (Writing)', NULL, 'BNB', 22, NULL, 0.00, 35.00, 'Active'),
(35, 'Clearbook (S)', NULL, 'CBS', 26, NULL, 0.00, 55.00, 'Active'),
(36, 'Oil Pastel 12\'s', NULL, 'OIL12', 28, NULL, 0.00, 90.00, 'Active'),
(37, 'Col.Pad 10', NULL, 'C10', 22, NULL, 0.00, 30.00, 'Active'),
(38, 'Oil Pastel 16\'s', NULL, 'OIL16', 28, NULL, 0.00, 90.00, 'Active'),
(39, 'Col.Pad 12', NULL, 'C12', 22, NULL, 0.00, 30.00, 'Active'),
(40, 'OJT Index Card', NULL, 'OP', 24, NULL, 0.00, 40.00, 'Active'),
(41, 'Col.Pad 14', NULL, 'C14', 22, NULL, 0.00, 30.00, 'Active'),
(42, 'Ordinary Folder (L)', NULL, 'OFL', 26, NULL, 0.00, 20.00, 'Active'),
(43, 'Col.Pad 16', NULL, 'C16', 22, NULL, 0.00, 30.00, 'Active'),
(44, 'Ordinary Folder (S)', NULL, 'OFS', 26, NULL, 0.00, 20.00, 'Active'),
(45, 'Col.Pad 2', NULL, 'C2', 22, NULL, 0.00, 30.00, 'Active'),
(46, 'Oslo Paper', NULL, 'OP', 22, NULL, 0.00, 10.00, 'Active'),
(47, 'Col.Pad 3', NULL, 'C3', 22, NULL, 0.00, 30.00, 'Active'),
(48, 'Packaging Tape 2\"', NULL, 'PT', 25, NULL, 0.00, 55.00, 'Active'),
(49, 'Col.Pad 4', NULL, 'C4', 22, NULL, 0.00, 30.00, 'Active'),
(50, 'Pad Paper G1', NULL, NULL, 22, NULL, 0.00, 30.00, 'Active'),
(51, 'Col.Pad 6', NULL, 'C6', 22, NULL, 0.00, 30.00, 'Active'),
(52, 'Pad Paper G2', NULL, NULL, 22, NULL, 0.00, 30.00, 'Active'),
(53, 'Col.Pad 8', NULL, 'C8', 22, NULL, 0.00, 30.00, 'Active'),
(54, 'Pad Paper G3', NULL, NULL, 22, NULL, 0.00, 30.00, 'Active'),
(55, 'Colored Folder (L)', NULL, 'CFL', 26, NULL, 0.00, 20.00, 'Active'),
(56, 'Pad Paper G4', NULL, NULL, 22, NULL, 0.00, 30.00, 'Active'),
(57, 'Colored Folder (S)', NULL, 'CFS', 26, NULL, 0.00, 20.00, 'Active'),
(58, 'Paint Brush (L)', NULL, 'PBL', 28, NULL, 0.00, 25.00, 'Active'),
(59, 'Colored Paper', NULL, 'CP', 22, NULL, 0.00, 10.00, 'Active'),
(60, 'Paint Brush (M)', NULL, 'PBM', 28, NULL, 0.00, 25.00, 'Active'),
(61, 'Columnar book 12', NULL, 'CB12', 24, NULL, 0.00, 40.00, 'Active'),
(62, 'Paint Brush (S)', NULL, 'PBS', 28, NULL, 0.00, 25.00, 'Active'),
(63, 'Columnar book 14', NULL, 'CB14', 24, NULL, 0.00, 40.00, 'Active'),
(64, 'Panda Bpen (B)', NULL, 'PANB', 8, NULL, 0.00, 12.00, 'Active'),
(65, 'Columnar book 16', NULL, 'CB16', 24, NULL, 0.00, 40.00, 'Active'),
(66, 'Panda Bpen (BL)', NULL, 'PANBL', 8, NULL, 0.00, 12.00, 'Active'),
(67, 'Compass', NULL, 'COMP', 29, NULL, 0.00, 25.00, 'Active'),
(68, 'Panda Bpen (R)', NULL, 'PANR', 8, NULL, 0.00, 12.00, 'Active'),
(69, 'Correction Tape', NULL, 'CT', 8, NULL, 0.00, 35.00, 'Active'),
(70, 'Panty Liner', NULL, 'PL', 30, NULL, 0.00, 35.00, 'Active'),
(71, 'Crayons 16\'s', NULL, 'CC16', 28, NULL, 0.00, 45.00, 'Active'),
(72, 'Paper Clip', NULL, 'PBB', 23, NULL, 0.00, 25.00, 'Active'),
(73, 'Crayons 8\'s', NULL, 'CC8', 28, NULL, 0.00, 45.00, 'Active'),
(74, 'Pencil Monggol', NULL, 'PEN', 8, NULL, 0.00, 12.00, 'Active'),
(75, 'Crepe Paper', NULL, 'CRP', 22, NULL, 0.00, 15.00, 'Active'),
(76, 'Periodic Table', NULL, 'PER', 31, NULL, 0.00, 35.00, 'Active'),
(77, 'Double sided-tape 1\"', NULL, 'DST1', 27, NULL, 0.00, 45.00, 'Active'),
(78, 'Permanent Marker (PILOT)', NULL, 'PPM', 8, NULL, 0.00, 35.00, 'Active'),
(79, 'Double sided-tape 1/2\"', NULL, 'DST1/2', 27, NULL, 0.00, 45.00, 'Active'),
(80, 'Phil. Map', NULL, 'PM', 31, NULL, 0.00, 35.00, 'Active'),
(81, 'Drawing Pad', NULL, 'DTR', 22, NULL, 0.00, 50.00, 'Active'),
(82, 'Plastic Cover / Yard', NULL, 'PC', 27, NULL, 0.00, 25.00, 'Active'),
(83, 'DTR', NULL, 'DTR', 24, NULL, 0.00, 40.00, 'Active'),
(84, 'Plastic Envelope (L)', NULL, 'PEL', 26, NULL, 0.00, 15.00, 'Active'),
(85, 'DTR (SPES)', NULL, 'DTR', 24, NULL, 0.00, 40.00, 'Active'),
(86, 'Plastic Envelope (S)', NULL, 'PES', 26, NULL, 0.00, 15.00, 'Active'),
(87, 'Eraser (Pencil)', NULL, 'ERA', 8, NULL, 0.00, 12.00, 'Active'),
(88, 'Poster Paint', NULL, 'HBP', 28, NULL, 0.00, 85.00, 'Active'),
(89, 'Eraser (White board)', NULL, 'ERAB', 8, NULL, 0.00, 15.00, 'Active'),
(90, 'Protractor', NULL, 'PRO', 29, NULL, 0.00, 25.00, 'Active'),
(91, 'Expanding Envelope (L)', NULL, 'EFL', 26, NULL, 0.00, 65.00, 'Active'),
(92, 'Quiz Notebook', NULL, 'QN', 22, NULL, 0.00, 35.00, 'Active'),
(93, 'Expanding Folder (L)', NULL, 'EFL', 26, NULL, 0.00, 65.00, 'Active'),
(94, 'Ruler', NULL, 'RULE', 29, NULL, 0.00, 25.00, 'Active'),
(95, 'Face Mask', NULL, 'FM', 30, NULL, 0.00, 10.00, 'Active'),
(96, 'Safety pin (Pardible)', NULL, 'PAR', 23, NULL, 0.00, 40.00, 'Active'),
(97, 'Fastener', NULL, 'FAS', 23, NULL, 0.00, 25.00, 'Active'),
(98, 'Scissors (B)', NULL, 'SCIB', 32, NULL, 0.00, 45.00, 'Active'),
(99, 'Folder cover (L)', NULL, 'FCL', 26, NULL, 0.00, 20.00, 'Active'),
(100, 'Scotch tape 1\" (B)', NULL, 'ST1/2', 27, NULL, 0.00, 45.00, 'Active'),
(101, 'Folder cover (S)', NULL, 'FCS', 26, NULL, 0.00, 20.00, 'Active'),
(102, 'Scotch tape 1\" (S)', NULL, 'ST1', 27, NULL, 0.00, 45.00, 'Active'),
(103, 'Gloves', NULL, 'GL', 30, NULL, 0.00, 25.00, 'Active'),
(104, 'Scotch tape 1/2\" (B)', NULL, 'ST2', 27, NULL, 0.00, 45.00, 'Active'),
(105, 'Gloves Food', NULL, 'GLF', 30, NULL, 0.00, 25.00, 'Active'),
(106, 'Scotch tape 1/2\" (S)', NULL, 'STS', 27, NULL, 0.00, 45.00, 'Active'),
(107, 'Glue stick (Big)', NULL, 'GSTB', 27, NULL, 0.00, 25.00, 'Active'),
(108, 'Scotch tape 2\"', NULL, 'ST2', 27, NULL, 0.00, 45.00, 'Active'),
(109, 'GlUe stick (Small)', NULL, 'GSTS', 27, NULL, 0.00, 25.00, 'Active'),
(110, 'Sewing Kit', NULL, 'SKIT', 33, NULL, 0.00, 75.00, 'Active'),
(111, 'Graphing paper', NULL, 'GP', 22, NULL, 0.00, 30.00, 'Active'),
(112, 'Sharpener', NULL, 'SHARP', 8, NULL, 0.00, 15.00, 'Active'),
(113, 'HBW Permanent Marker (Black)', NULL, NULL, 8, NULL, 0.00, 35.00, 'Active'),
(114, 'Sliding Folder (L)', NULL, 'SFL', 26, NULL, 0.00, 20.00, 'Active'),
(115, 'HBW Permanent Marker (Blue)', NULL, NULL, 8, NULL, 0.00, 35.00, 'Active'),
(116, 'Sliding Folder (S)', NULL, 'SFS', 26, NULL, 0.00, 20.00, 'Active'),
(117, 'HBW Permanent Marker (Red)', NULL, NULL, 8, NULL, 0.00, 35.00, 'Active'),
(118, 'Special Paper', NULL, 'SP', 22, NULL, 0.00, 30.00, 'Active'),
(119, 'ID case', NULL, NULL, 34, NULL, 0.00, 80.00, 'Active'),
(120, 'Thread (Sinulid) B', NULL, 'THB', 33, NULL, 0.00, 30.00, 'Active'),
(121, 'Illustration Board 1/4', NULL, '1/B 1/4', 22, NULL, 0.00, 35.00, 'Active'),
(122, 'Thread (Sinulid) S', NULL, 'THS', 33, NULL, 0.00, 30.00, 'Active'),
(123, 'Illustration Board 1/8', NULL, 'I/B 1/8', 22, NULL, 0.00, 35.00, 'Active'),
(124, 'Thumb Tacks', NULL, 'TBT', 23, NULL, 0.00, 25.00, 'Active'),
(125, 'Index Card 1/2', NULL, 'IC 1/2', 22, NULL, 0.00, 25.00, 'Active'),
(126, 'Tissue', NULL, 'TISS', 30, NULL, 0.00, 30.00, 'Active'),
(127, 'Index Card 1/2 (Blue)', NULL, 'IC 1/2', 22, NULL, 0.00, 25.00, 'Active'),
(128, 'Water Color', NULL, 'WC', 28, NULL, 0.00, 85.00, 'Active'),
(129, 'Index Card 1/2 (Pink)', NULL, 'IC 1/2', 22, NULL, 0.00, 25.00, 'Active'),
(130, 'White Board Marker (Black)', NULL, 'WB', 8, NULL, 0.00, 35.00, 'Active'),
(131, 'Index Card 1/2 (Violet)', NULL, 'IC 1/2', 22, NULL, 0.00, 25.00, 'Active'),
(132, 'White Board Marker (Red)', NULL, 'WB', 8, NULL, 0.00, 35.00, 'Active'),
(133, 'Index Card 1/4', NULL, NULL, 22, NULL, 0.00, 25.00, 'Active'),
(134, 'Wipes', NULL, 'WPS', 30, NULL, 0.00, 30.00, 'Active'),
(135, 'Index Card 1/4 (Blue)', NULL, NULL, 22, NULL, 0.00, 25.00, 'Active'),
(136, 'World Map', NULL, 'WM', 31, NULL, 0.00, 35.00, 'Active'),
(137, 'Index Card 1/4 (Pink)', NULL, NULL, 22, NULL, 0.00, 25.00, 'Active'),
(138, 'Yarn', NULL, 'YN', 33, NULL, 0.00, 30.00, 'Active'),
(139, 'Index Card 1/4 (Yellow)', NULL, 'IC 1/8', 22, NULL, 0.00, 25.00, 'Active'),
(140, 'Yellow Pad (Whole)', NULL, 'YP', 22, NULL, 0.00, 55.00, 'Active'),
(141, 'Index Card 1/4 (Violet)', NULL, 'IC 1/8', 22, NULL, 0.00, 25.00, 'Active'),
(142, 'Yellow PCS', NULL, 'YP', 22, NULL, 0.00, 30.00, 'Active'),
(143, 'Index Card 1/8', NULL, 'IC 1/8', 22, NULL, 0.00, 25.00, 'Active'),
(144, 'Index Card 1/8 (Yellow)', NULL, 'IC 1/8', 22, NULL, 0.00, 25.00, 'Active'),
(145, 'Index Card 1/8 (Green)', NULL, 'IC 1/8', 22, NULL, 0.00, 25.00, 'Active'),
(146, 'Index Card 1/8 (Violet)', NULL, 'IC 1/8', 22, NULL, 0.00, 25.00, 'Active'),
(147, 'Index Card 1/8 (Pink)', NULL, 'IC 1/8', 22, NULL, 0.00, 25.00, 'Active'),
(148, 'Intermediate Pad', NULL, 'IP', 22, NULL, 0.00, 45.00, 'Active'),
(149, 'SU POLO', NULL, NULL, 35, NULL, 0.00, 320.00, 'Active'),
(150, 'SU BLOUSE', NULL, NULL, 35, NULL, 0.00, 320.00, 'Active'),
(151, 'SU X-JUMPER', NULL, NULL, 35, NULL, 0.00, 380.00, 'Active'),
(152, 'SU JUMP SKIRT', NULL, NULL, 35, NULL, 0.00, 320.00, 'Active'),
(153, 'PE J-PANTS', NULL, NULL, 35, NULL, 0.00, 350.00, 'Active'),
(154, 'PE T-SHIRTS', NULL, NULL, 35, NULL, 0.00, 280.00, 'Active'),
(155, 'SU X-JUMPER (BS VERSION)', NULL, NULL, 35, NULL, 0.00, 380.00, 'Active'),
(156, 'SU SHORTS', NULL, NULL, 35, NULL, 0.00, 250.00, 'Active'),
(157, 'SU POLO', NULL, NULL, 36, NULL, 0.00, 320.00, 'Active'),
(158, 'SU BLOUSE', NULL, NULL, 36, NULL, 0.00, 320.00, 'Active'),
(159, 'PE J-PANTS', NULL, NULL, 36, NULL, 0.00, 350.00, 'Active'),
(160, 'PE T-SHIRTS', NULL, NULL, 36, NULL, 0.00, 280.00, 'Active'),
(161, 'SU PANTS', NULL, NULL, 36, NULL, 0.00, 350.00, 'Active'),
(162, 'SU SKIRT', NULL, NULL, 36, NULL, 0.00, 320.00, 'Active'),
(163, 'VEST SHS (FEMALE)', NULL, NULL, 36, NULL, 0.00, 380.00, 'Active'),
(164, 'VEST SHS (MALE)', NULL, NULL, 36, NULL, 0.00, 380.00, 'Active'),
(165, 'CHEF\'S COAT SHS', NULL, NULL, 36, NULL, 0.00, 550.00, 'Active'),
(166, 'SU POLO BARONG', NULL, NULL, 37, NULL, 0.00, 450.00, 'Active'),
(167, 'SU BLOUSE', NULL, NULL, 37, NULL, 0.00, 320.00, 'Active'),
(168, 'PE J-PANTS', NULL, NULL, 37, NULL, 0.00, 350.00, 'Active'),
(169, 'PE T-SHIRTS', NULL, NULL, 37, NULL, 0.00, 280.00, 'Active'),
(170, 'HM/TM SKIRT', NULL, NULL, 38, NULL, 0.00, 320.00, 'Active'),
(171, 'HM BLOUSE', NULL, NULL, 38, NULL, 0.00, 320.00, 'Active'),
(172, 'HM/TM COAT MALE', NULL, NULL, 38, NULL, 0.00, 550.00, 'Active'),
(173, 'HM/TM POLO', NULL, NULL, 38, NULL, 0.00, 320.00, 'Active'),
(174, 'TM BLAZER', NULL, NULL, 38, NULL, 0.00, 850.00, 'Active'),
(175, 'TM BLOUSE', NULL, NULL, 38, NULL, 0.00, 320.00, 'Active'),
(176, 'HM/TM PANTS', NULL, NULL, 38, NULL, 0.00, 350.00, 'Active'),
(177, 'HM BLAZER', NULL, NULL, 38, NULL, 0.00, 850.00, 'Active'),
(178, 'HM VEST (MALE)', NULL, NULL, 38, NULL, 0.00, 380.00, 'Active'),
(179, 'HM VEST (FEMALE)', NULL, NULL, 38, NULL, 0.00, 380.00, 'Active'),
(180, 'CHEF\'S COAT', NULL, NULL, 38, NULL, 0.00, 550.00, 'Active'),
(181, 'BSCA POLO/BLOUSE (OLD)', NULL, NULL, 38, NULL, 0.00, 320.00, 'Active'),
(182, 'BSCA PANTS', NULL, NULL, 38, NULL, 0.00, 350.00, 'Active'),
(183, 'BSCA SKIRT', NULL, NULL, 38, NULL, 0.00, 320.00, 'Active'),
(184, 'BSCA POLO/BLOUSE (NEW)', NULL, NULL, 38, NULL, 0.00, 320.00, 'Active'),
(185, 'BSA BLAZER (MALE)', NULL, NULL, 38, NULL, 0.00, 850.00, 'Active'),
(186, 'BSA BLAZER (FEMALE)', NULL, NULL, 38, NULL, 0.00, 850.00, 'Active'),
(187, 'BSBA BLAZER (MALE)', NULL, NULL, 38, NULL, 0.00, 850.00, 'Active'),
(188, 'BSBA BLAZER (FEMALE)', NULL, NULL, 38, NULL, 0.00, 850.00, 'Active'),
(189, 'PSYCHOLOGY POLO', NULL, NULL, 38, NULL, 0.00, 320.00, 'Active'),
(190, 'PSYCHOLOGY PANTS', NULL, NULL, 38, NULL, 0.00, 350.00, 'Active'),
(191, 'PSYCHOLOGY BLOUSE', NULL, NULL, 38, NULL, 0.00, 320.00, 'Active'),
(192, 'PSYCHOLOGY SKIRT', NULL, NULL, 38, NULL, 0.00, 320.00, 'Active'),
(193, 'ENGR COAT (M)', NULL, NULL, 38, NULL, 0.00, 550.00, 'Active'),
(194, 'ENGR PANTS (MALE)', NULL, NULL, 38, NULL, 0.00, 350.00, 'Active'),
(195, 'ENGR PANTS (FEMALE)', NULL, NULL, 38, NULL, 0.00, 350.00, 'Active'),
(196, 'ENGR BLAZER (F)', NULL, NULL, 38, NULL, 0.00, 850.00, 'Active'),
(197, 'EDUC POLO', NULL, NULL, 38, NULL, 0.00, 320.00, 'Active'),
(198, 'EDUC BLOUSE', NULL, NULL, 38, NULL, 0.00, 320.00, 'Active'),
(199, 'CCS BLAZER (FEMALE)', NULL, NULL, 38, NULL, 0.00, 850.00, 'Active'),
(200, 'CCS BLAZER (MALE)', NULL, NULL, 38, NULL, 0.00, 850.00, 'Active'),
(201, 'CRIMINOLOGY PANTS', NULL, NULL, 38, NULL, 0.00, 350.00, 'Active'),
(202, 'CRIMINOLOGY POLO', NULL, NULL, 38, NULL, 0.00, 320.00, 'Active'),
(203, 'ROTC Upper Fatigue', NULL, NULL, 39, NULL, 0.00, 500.00, 'Active'),
(204, 'ROTC PANTS Fatigue', NULL, NULL, 39, NULL, 0.00, 450.00, 'Active'),
(205, 'OJT-SHIRTS', NULL, NULL, 39, NULL, 0.00, 280.00, 'Active'),
(206, 'NSTP CWTS T-SHIRTS', NULL, NULL, 39, NULL, 0.00, 280.00, 'Active'),
(207, 'ROTC FATIGUE (SET)', NULL, NULL, 39, NULL, 0.00, 300.00, 'Active'),
(208, 'CUFFLINKS', NULL, NULL, 40, NULL, 0.00, 150.00, 'Active'),
(209, 'BELT SHS', NULL, NULL, 40, NULL, 0.00, 180.00, 'Active'),
(210, 'NECKTIE COLLEGE', NULL, NULL, 40, NULL, 0.00, 120.00, 'Active'),
(211, 'NECKTIE SHS', NULL, NULL, 40, NULL, 0.00, 120.00, 'Active'),
(212, 'RIBBON ELEMENTARY', NULL, NULL, 40, NULL, 0.00, 120.00, 'Active'),
(213, 'CRIMINOLOGY CAP', NULL, NULL, 40, NULL, 0.00, 180.00, 'Active'),
(214, 'BSBA NECKTIE (MALE)', NULL, NULL, 40, NULL, 0.00, 120.00, 'Active'),
(215, 'BSBA NECKTIE (FEMALE)', NULL, NULL, 40, NULL, 0.00, 120.00, 'Active'),
(216, 'BSA NECKTIE', NULL, NULL, 40, NULL, 0.00, 120.00, 'Active'),
(217, 'BSCA NECKTIE', NULL, NULL, 40, NULL, 0.00, 120.00, 'Active'),
(218, 'HM/TM SCARF', NULL, NULL, 40, NULL, 0.00, 120.00, 'Active'),
(219, 'CRIM PATCH', NULL, NULL, 40, NULL, 0.00, 80.00, 'Active'),
(220, 'CRIM. SHOULDER BOARD', NULL, NULL, 40, NULL, 0.00, 180.00, 'Active'),
(221, 'BSCA SHOULDER BOARD', NULL, NULL, 40, NULL, 0.00, 180.00, 'Active'),
(222, 'HM BLAZER W NAME (M)', NULL, NULL, 40, NULL, 0.00, 850.00, 'Active'),
(223, 'HM BLAZER W NAME (F)', NULL, NULL, 40, NULL, 0.00, 850.00, 'Active'),
(224, 'KATAPATAN PATCH', NULL, NULL, 40, NULL, 0.00, 80.00, 'Active'),
(225, 'CTHM NECKTIE', NULL, NULL, 40, NULL, 0.00, 120.00, 'Active'),
(226, 'RESERVE PATCH', NULL, NULL, 40, NULL, 0.00, 80.00, 'Active'),
(227, 'HUKBONG KATIHAN PATCH', NULL, NULL, 40, NULL, 0.00, 80.00, 'Active'),
(228, 'Sample Grade 7 English Textbook', NULL, NULL, 12, NULL, 0.00, 350.00, 'Active'),
(229, 'Sample General Math Module', NULL, NULL, 7, NULL, 0.00, 120.00, 'Active'),
(230, 'Basic Ed - Grade 7 English Communication Textbook', NULL, NULL, 12, NULL, 0.00, 380.00, 'Active'),
(231, 'Basic Ed - Grade 8 Science & Technology Textbook', NULL, NULL, 12, NULL, 0.00, 420.00, 'Active'),
(232, 'Basic Ed - Grade 9 Mathematics & Algebra Module', NULL, NULL, 7, NULL, 0.00, 250.00, 'Active'),
(233, 'Basic Ed - Grade 10 Araling Panlipunan Learning Module', NULL, NULL, 7, NULL, 0.00, 230.00, 'Active'),
(234, 'SHS - Practical Research 1 & 2 Learning Module', NULL, NULL, 7, NULL, 0.00, 180.00, 'Active'),
(235, 'SHS - Fundamentals of Accountancy, Business & Management (ABM)', NULL, NULL, 12, NULL, 0.00, 450.00, 'Active'),
(236, 'SHS - General Mathematics & Pre-Calculus (STEM)', NULL, NULL, 12, NULL, 0.00, 480.00, 'Active'),
(237, 'SHS - Empowerment Technologies & ICT Module', NULL, NULL, 7, NULL, 0.00, 195.00, 'Active'),
(238, 'SHS - Computer Systems Servicing NC II Worktext', NULL, NULL, 12, NULL, 0.00, 350.00, 'Active'),
(239, 'CCS - Object-Oriented Programming (Java / C#) Module', NULL, NULL, 7, NULL, 0.00, 220.00, 'Active'),
(240, 'CCS - Data Structures and Algorithms Courseware', NULL, NULL, 12, NULL, 0.00, 550.00, 'Active'),
(241, 'CCS - Database Management Systems & MySQL Laboratory Manual', NULL, NULL, 7, NULL, 0.00, 250.00, 'Active'),
(242, 'CCS - Web Systems and Technologies Course Module', NULL, NULL, 7, NULL, 0.00, 210.00, 'Active'),
(243, 'CBME - Financial Management & Accounting Principles', NULL, NULL, 12, NULL, 0.00, 520.00, 'Active'),
(244, 'CBME - Customs Laws, Rules and Regulations Guidebook', NULL, NULL, 12, NULL, 0.00, 600.00, 'Active'),
(245, 'CBME - Marketing Management Principles Module', NULL, NULL, 7, NULL, 0.00, 240.00, 'Active'),
(246, 'CCJ - Introduction to Criminology & Criminalistics Manual', NULL, NULL, 12, NULL, 0.00, 490.00, 'Active'),
(247, 'COE - Differential & Integral Calculus Coursebook', NULL, NULL, 12, NULL, 0.00, 580.00, 'Active'),
(248, 'TESDA - Bookkeeping NC II Training Module', NULL, NULL, 7, NULL, 0.00, 160.00, 'Active'),
(249, 'TESDA - Cookery & Food Service Management Manual', NULL, NULL, 7, NULL, 0.00, 280.00, 'Active');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_product_variants`
--

CREATE TABLE `tbl_product_variants` (
  `variant_id` int(11) NOT NULL,
  `product_id` int(11) NOT NULL,
  `product_code` varchar(50) NOT NULL,
  `size` varchar(20) NOT NULL DEFAULT 'N/A',
  `quantity_on_hand` int(11) NOT NULL DEFAULT 0,
  `reorder_level` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_product_variants`
--

INSERT INTO `tbl_product_variants` (`variant_id`, `product_id`, `product_code`, `size`, `quantity_on_hand`, `reorder_level`) VALUES
(1, 1, 'SUP-ART', 'N/A', 5, 20),
(2, 2, 'SUP-IPC-1-2', 'N/A', 100, 20),
(3, 3, 'SUP-BC-1-4', 'N/A', 49, 10),
(4, 4, 'SUP-IPL-1-2', 'N/A', 100, 20),
(5, 5, 'SUP-BC-5-8', 'N/A', 49, 10),
(6, 6, 'SUP-JP-01', 'N/A', 50, 10),
(7, 7, 'SUP-BC-1', 'N/A', 3, 10),
(8, 8, 'SUP-JP-02', 'N/A', 100, 20),
(9, 9, 'SUP-BC-2', 'N/A', 50, 10),
(10, 10, 'SUP-LAM', 'N/A', 50, 10),
(11, 11, 'SUP-BB', 'N/A', 59, 15),
(12, 12, 'SUP-LS', 'N/A', 50, 10),
(13, 13, 'SUP-BPL-01', 'N/A', 100, 20),
(14, 14, 'SUP-LE', 'N/A', 50, 10),
(15, 15, 'SUP-BPL-02', 'N/A', 100, 20),
(16, 16, 'SUP-MP', 'N/A', 100, 20),
(17, 17, 'SUP-BPS', 'N/A', 100, 20),
(18, 18, 'SUP-0018', 'N/A', 60, 15),
(19, 19, 'SUP-HBWB', 'N/A', 60, 15),
(20, 20, 'SUP-0020', 'N/A', 60, 15),
(21, 21, 'SUP-HBWR', 'N/A', 60, 15),
(22, 22, 'SUP-MNB', 'N/A', 100, 20),
(23, 23, 'SUP-BEL', 'N/A', 50, 10),
(24, 24, 'SUP-MCL', 'N/A', 50, 10),
(25, 25, 'SUP-BES', 'N/A', 50, 10),
(26, 26, 'SUP-MCS', 'N/A', 50, 10),
(27, 27, 'SUP-CART', 'N/A', 60, 15),
(28, 28, 'SUP-MOROL', 'N/A', 50, 10),
(29, 29, 'SUP-CARB', 'N/A', 60, 15),
(30, 30, 'SUP-MOROS', 'N/A', 50, 10),
(31, 31, 'SUP-CAT', 'N/A', 60, 15),
(32, 32, 'SUP-NB', 'N/A', 100, 20),
(33, 33, 'SUP-CBL', 'N/A', 50, 10),
(34, 34, 'SUP-BNB', 'N/A', 100, 20),
(35, 35, 'SUP-CBS', 'N/A', 50, 10),
(36, 36, 'SUP-OIL12', 'N/A', 60, 15),
(37, 37, 'SUP-C10', 'N/A', 100, 20),
(38, 38, 'SUP-OIL16', 'N/A', 60, 15),
(39, 39, 'SUP-C12', 'N/A', 100, 20),
(40, 40, 'SUP-OP-01', 'N/A', 50, 10),
(41, 41, 'SUP-C14', 'N/A', 100, 20),
(42, 42, 'SUP-OFL', 'N/A', 50, 10),
(43, 43, 'SUP-C16', 'N/A', 100, 20),
(44, 44, 'SUP-OFS', 'N/A', 50, 10),
(45, 45, 'SUP-C2', 'N/A', 100, 20),
(46, 46, 'SUP-OP-02', 'N/A', 100, 20),
(47, 47, 'SUP-C3', 'N/A', 100, 20),
(48, 48, 'SUP-PT', 'N/A', 50, 10),
(49, 49, 'SUP-C4', 'N/A', 100, 20),
(50, 50, 'SUP-0050', 'N/A', 100, 20),
(51, 51, 'SUP-C6', 'N/A', 100, 20),
(52, 52, 'SUP-0052', 'N/A', 100, 20),
(53, 53, 'SUP-C8', 'N/A', 100, 20),
(54, 54, 'SUP-0054', 'N/A', 100, 20),
(55, 55, 'SUP-CFL', 'N/A', 50, 10),
(56, 56, 'SUP-0056', 'N/A', 100, 20),
(57, 57, 'SUP-CFS', 'N/A', 50, 10),
(58, 58, 'SUP-PBL', 'N/A', 60, 15),
(59, 59, 'SUP-CP', 'N/A', 100, 20),
(60, 60, 'SUP-PBM', 'N/A', 60, 15),
(61, 61, 'SUP-CB12', 'N/A', 50, 10),
(62, 62, 'SUP-PBS', 'N/A', 60, 15),
(63, 63, 'SUP-CB14', 'N/A', 50, 10),
(64, 64, 'SUP-PANB', 'N/A', 60, 15),
(65, 65, 'SUP-CB16', 'N/A', 50, 10),
(66, 66, 'SUP-PANBL', 'N/A', 60, 15),
(67, 67, 'SUP-COMP', 'N/A', 60, 15),
(68, 68, 'SUP-PANR', 'N/A', 60, 15),
(69, 69, 'SUP-CT', 'N/A', 60, 15),
(70, 70, 'SUP-PL', 'N/A', 40, 10),
(71, 71, 'SUP-CC16', 'N/A', 60, 15),
(72, 72, 'SUP-PBB', 'N/A', 50, 10),
(73, 73, 'SUP-CC8', 'N/A', 60, 15),
(74, 74, 'SUP-PEN', 'N/A', 100, 20),
(75, 75, 'SUP-CRP', 'N/A', 100, 20),
(76, 76, 'SUP-PER', 'N/A', 60, 15),
(77, 77, 'SUP-DST1', 'N/A', 60, 15),
(78, 78, 'SUP-PPM', 'N/A', 60, 15),
(79, 79, 'SUP-DST1-2', 'N/A', 60, 15),
(80, 80, 'SUP-PM', 'N/A', 60, 15),
(81, 81, 'SUP-DTR-01', 'N/A', 100, 20),
(82, 82, 'SUP-PC', 'N/A', 60, 15),
(83, 83, 'SUP-DTR-02', 'N/A', 50, 10),
(84, 84, 'SUP-PEL', 'N/A', 50, 10),
(85, 85, 'SUP-DTR-03', 'N/A', 50, 10),
(86, 86, 'SUP-PES', 'N/A', 50, 10),
(87, 87, 'SUP-ERA', 'N/A', 100, 20),
(88, 88, 'SUP-HBP', 'N/A', 60, 15),
(89, 89, 'SUP-ERAB', 'N/A', 60, 15),
(90, 90, 'SUP-PRO', 'N/A', 60, 15),
(91, 91, 'SUP-EFL-01', 'N/A', 50, 10),
(92, 92, 'SUP-QN', 'N/A', 100, 20),
(93, 93, 'SUP-EFL-02', 'N/A', 50, 10),
(94, 94, 'SUP-RULE', 'N/A', 60, 15),
(95, 95, 'SUP-FM', 'N/A', 40, 10),
(96, 96, 'SUP-PAR', 'N/A', 50, 10),
(97, 97, 'SUP-FAS', 'N/A', 50, 10),
(98, 98, 'SUP-SCIB', 'N/A', 60, 15),
(99, 99, 'SUP-FCL', 'N/A', 50, 10),
(100, 100, 'SUP-ST1-2', 'N/A', 60, 15),
(101, 101, 'SUP-FCS', 'N/A', 50, 10),
(102, 102, 'SUP-ST1', 'N/A', 60, 15),
(103, 103, 'SUP-GL', 'N/A', 40, 10),
(104, 104, 'SUP-ST2-01', 'N/A', 60, 15),
(105, 105, 'SUP-GLF', 'N/A', 40, 10),
(106, 106, 'SUP-STS', 'N/A', 60, 15),
(107, 107, 'SUP-GSTB', 'N/A', 60, 15),
(108, 108, 'SUP-ST2-02', 'N/A', 60, 15),
(109, 109, 'SUP-GSTS', 'N/A', 60, 15),
(110, 110, 'SUP-SKIT', 'N/A', 40, 10),
(111, 111, 'SUP-GP', 'N/A', 100, 20),
(112, 112, 'SUP-SHARP', 'N/A', 60, 15),
(113, 113, 'SUP-0113', 'N/A', 60, 15),
(114, 114, 'SUP-SFL', 'N/A', 50, 10),
(115, 115, 'SUP-0115', 'N/A', 60, 15),
(116, 116, 'SUP-SFS', 'N/A', 50, 10),
(117, 117, 'SUP-0117', 'N/A', 60, 15),
(118, 118, 'SUP-SP', 'N/A', 100, 20),
(119, 119, 'SUP-0119', 'N/A', 40, 10),
(120, 120, 'SUP-THB', 'N/A', 40, 10),
(121, 121, 'SUP-1-B-1-4', 'N/A', 60, 15),
(122, 122, 'SUP-THS', 'N/A', 40, 10),
(123, 123, 'SUP-I-B-1-8', 'N/A', 60, 15),
(124, 124, 'SUP-TBT', 'N/A', 50, 10),
(125, 125, 'SUP-IC-1-2-01', 'N/A', 60, 15),
(126, 126, 'SUP-TISS', 'N/A', 40, 10),
(127, 127, 'SUP-IC-1-2-02', 'N/A', 60, 15),
(128, 128, 'SUP-WC', 'N/A', 60, 15),
(129, 129, 'SUP-IC-1-2-03', 'N/A', 60, 15),
(130, 130, 'SUP-WB-01', 'N/A', 60, 15),
(131, 131, 'SUP-IC-1-2-04', 'N/A', 60, 15),
(132, 132, 'SUP-WB-02', 'N/A', 60, 15),
(133, 133, 'SUP-0133', 'N/A', 60, 15),
(134, 134, 'SUP-WPS', 'N/A', 40, 10),
(135, 135, 'SUP-0135', 'N/A', 60, 15),
(136, 136, 'SUP-WM', 'N/A', 60, 15),
(137, 137, 'SUP-0137', 'N/A', 60, 15),
(138, 138, 'SUP-YN', 'N/A', 40, 10),
(139, 139, 'SUP-IC-1-8-01', 'N/A', 60, 15),
(140, 140, 'SUP-YP-01', 'N/A', 100, 20),
(141, 141, 'SUP-IC-1-8-02', 'N/A', 60, 15),
(142, 142, 'SUP-YP-02', 'N/A', 60, 15),
(143, 143, 'SUP-IC-1-8-03', 'N/A', 60, 15),
(144, 144, 'SUP-IC-1-8-04', 'N/A', 60, 15),
(145, 145, 'SUP-IC-1-8-05', 'N/A', 60, 15),
(146, 146, 'SUP-IC-1-8-06', 'N/A', 60, 15),
(147, 147, 'SUP-IC-1-8-07', 'N/A', 60, 15),
(148, 148, 'SUP-IP', 'N/A', 100, 20),
(149, 149, 'UNI-KE-001-S', 'S', 20, 5),
(150, 149, 'UNI-KE-001-M', 'M', 20, 5),
(151, 149, 'UNI-KE-001-L', 'L', 20, 5),
(152, 149, 'UNI-KE-001-XL', 'XL', 20, 5),
(153, 149, 'UNI-KE-001-2XL', '2XL', 20, 5),
(154, 149, 'UNI-KE-001-3XL', '3XL', 20, 5),
(155, 149, 'UNI-KE-001-4XL', '4XL', 20, 5),
(156, 149, 'UNI-KE-001-5XL', '5XL', 20, 5),
(157, 149, 'UNI-KE-001-6XL', '6XL', 20, 5),
(158, 149, 'UNI-KE-001-7XL', '7XL', 20, 5),
(159, 150, 'UNI-KE-002-XS', 'XS', 20, 5),
(160, 150, 'UNI-KE-002-S', 'S', 20, 5),
(161, 150, 'UNI-KE-002-M', 'M', 20, 5),
(162, 150, 'UNI-KE-002-L', 'L', 20, 5),
(163, 150, 'UNI-KE-002-XL', 'XL', 20, 5),
(164, 150, 'UNI-KE-002-2XL', '2XL', 20, 5),
(165, 150, 'UNI-KE-002-3XL', '3XL', 20, 5),
(166, 150, 'UNI-KE-002-4XL', '4XL', 20, 5),
(167, 150, 'UNI-KE-002-5XL', '5XL', 20, 5),
(168, 150, 'UNI-KE-002-6XL', '6XL', 20, 5),
(169, 151, 'UNI-KE-003-XS', 'XS', 20, 5),
(170, 151, 'UNI-KE-003-S', 'S', 20, 5),
(171, 151, 'UNI-KE-003-M', 'M', 20, 5),
(172, 151, 'UNI-KE-003-L', 'L', 20, 5),
(173, 151, 'UNI-KE-003-XL', 'XL', 20, 5),
(174, 151, 'UNI-KE-003-2XL', '2XL', 20, 5),
(175, 151, 'UNI-KE-003-3XL', '3XL', 20, 5),
(176, 151, 'UNI-KE-003-4XL', '4XL', 20, 5),
(177, 151, 'UNI-KE-003-5XL', '5XL', 20, 5),
(178, 151, 'UNI-KE-003-6XL', '6XL', 20, 5),
(179, 152, 'UNI-KE-004-XS', 'XS', 20, 5),
(180, 152, 'UNI-KE-004-S', 'S', 20, 5),
(181, 152, 'UNI-KE-004-M', 'M', 20, 5),
(182, 152, 'UNI-KE-004-L', 'L', 20, 5),
(183, 152, 'UNI-KE-004-XL', 'XL', 20, 5),
(184, 152, 'UNI-KE-004-2XL', '2XL', 20, 5),
(185, 152, 'UNI-KE-004-3XL', '3XL', 20, 5),
(186, 152, 'UNI-KE-004-4XL', '4XL', 20, 5),
(187, 152, 'UNI-KE-004-5XL', '5XL', 20, 5),
(188, 152, 'UNI-KE-004-6XL', '6XL', 20, 5),
(189, 153, 'UNI-KE-005-S8', 'S8', 20, 5),
(190, 153, 'UNI-KE-005-S10', 'S10', 20, 5),
(191, 153, 'UNI-KE-005-S12', 'S12', 20, 5),
(192, 153, 'UNI-KE-005-S14', 'S14', 20, 5),
(193, 153, 'UNI-KE-005-S16', 'S16', 20, 5),
(194, 153, 'UNI-KE-005-S18', 'S18', 20, 5),
(195, 153, 'UNI-KE-005-S20', 'S20', 20, 5),
(196, 153, 'UNI-KE-005-S22', 'S22', 20, 5),
(197, 153, 'UNI-KE-005-L', 'L', 20, 5),
(198, 153, 'UNI-KE-005-XL', 'XL', 20, 5),
(199, 153, 'UNI-KE-005-2XL', '2XL', 20, 5),
(200, 153, 'UNI-KE-005-3XL', '3XL', 20, 5),
(201, 153, 'UNI-KE-005-4XL', '4XL', 20, 5),
(202, 153, 'UNI-KE-005-5XL', '5XL', 20, 5),
(203, 154, 'UNI-KE-006-S8', 'S8', 20, 5),
(204, 154, 'UNI-KE-006-S10', 'S10', 20, 5),
(205, 154, 'UNI-KE-006-S12', 'S12', 20, 5),
(206, 154, 'UNI-KE-006-S14', 'S14', 20, 5),
(207, 154, 'UNI-KE-006-S16', 'S16', 20, 5),
(208, 154, 'UNI-KE-006-S18', 'S18', 20, 5),
(209, 154, 'UNI-KE-006-S20', 'S20', 20, 5),
(210, 154, 'UNI-KE-006-S22', 'S22', 20, 5),
(211, 154, 'UNI-KE-006-S24', 'S24', 20, 5),
(212, 154, 'UNI-KE-006-S', 'S', 20, 5),
(213, 154, 'UNI-KE-006-M', 'M', 20, 5),
(214, 154, 'UNI-KE-006-L', 'L', 20, 5),
(215, 154, 'UNI-KE-006-XL', 'XL', 20, 5),
(216, 155, 'UNI-KE-007-XS', 'XS', 20, 5),
(217, 155, 'UNI-KE-007-S', 'S', 20, 5),
(218, 155, 'UNI-KE-007-M', 'M', 20, 5),
(219, 155, 'UNI-KE-007-L', 'L', 20, 5),
(220, 155, 'UNI-KE-007-XL', 'XL', 20, 5),
(221, 155, 'UNI-KE-007-2XL', '2XL', 20, 5),
(222, 155, 'UNI-KE-007-3XL', '3XL', 20, 5),
(223, 155, 'UNI-KE-007-4XL', '4XL', 20, 5),
(224, 155, 'UNI-KE-007-5XL', '5XL', 20, 5),
(225, 155, 'UNI-KE-007-6XL', '6XL', 20, 5),
(226, 156, 'UNI-KE-008-S24', 'S24', 20, 5),
(227, 156, 'UNI-KE-008-XS', 'XS', 20, 5),
(228, 156, 'UNI-KE-008-S', 'S', 20, 5),
(229, 156, 'UNI-KE-008-M', 'M', 20, 5),
(230, 156, 'UNI-KE-008-L', 'L', 20, 5),
(231, 156, 'UNI-KE-008-XL', 'XL', 20, 5),
(232, 156, 'UNI-KE-008-2XL', '2XL', 20, 5),
(233, 156, 'UNI-KE-008-3XL', '3XL', 20, 5),
(234, 156, 'UNI-KE-008-4XL', '4XL', 20, 5),
(235, 156, 'UNI-KE-008-5XL', '5XL', 20, 5),
(236, 156, 'UNI-KE-008-6XL', '6XL', 20, 5),
(237, 157, 'UNI-JS-009-XS', 'XS', 20, 5),
(238, 157, 'UNI-JS-009-S', 'S', 20, 5),
(239, 157, 'UNI-JS-009-M', 'M', 20, 5),
(240, 157, 'UNI-JS-009-L', 'L', 20, 5),
(241, 157, 'UNI-JS-009-XL', 'XL', 20, 5),
(242, 157, 'UNI-JS-009-2XL', '2XL', 20, 5),
(243, 157, 'UNI-JS-009-3XL', '3XL', 20, 5),
(244, 157, 'UNI-JS-009-4XL', '4XL', 20, 5),
(245, 157, 'UNI-JS-009-5XL', '5XL', 20, 5),
(246, 157, 'UNI-JS-009-6XL', '6XL', 20, 5),
(247, 158, 'UNI-JS-010-XS', 'XS', 20, 5),
(248, 158, 'UNI-JS-010-S', 'S', 20, 5),
(249, 158, 'UNI-JS-010-M', 'M', 20, 5),
(250, 158, 'UNI-JS-010-L', 'L', 20, 5),
(251, 158, 'UNI-JS-010-XL', 'XL', 20, 5),
(252, 158, 'UNI-JS-010-2XL', '2XL', 20, 5),
(253, 158, 'UNI-JS-010-3XL', '3XL', 20, 5),
(254, 158, 'UNI-JS-010-4XL', '4XL', 20, 5),
(255, 158, 'UNI-JS-010-5XL', '5XL', 20, 5),
(256, 158, 'UNI-JS-010-6XL', '6XL', 20, 5),
(257, 159, 'UNI-JS-011-XS', 'XS', 20, 5),
(258, 159, 'UNI-JS-011-S', 'S', 20, 5),
(259, 159, 'UNI-JS-011-M', 'M', 20, 5),
(260, 159, 'UNI-JS-011-L', 'L', 20, 5),
(261, 159, 'UNI-JS-011-XL', 'XL', 20, 5),
(262, 159, 'UNI-JS-011-2XL', '2XL', 20, 5),
(263, 159, 'UNI-JS-011-3XL', '3XL', 20, 5),
(264, 159, 'UNI-JS-011-4XL', '4XL', 20, 5),
(265, 159, 'UNI-JS-011-5XL', '5XL', 20, 5),
(266, 159, 'UNI-JS-011-6XL', '6XL', 20, 5),
(267, 160, 'UNI-JS-012-XS', 'XS', 20, 5),
(268, 160, 'UNI-JS-012-S', 'S', 20, 5),
(269, 160, 'UNI-JS-012-M', 'M', 20, 5),
(270, 160, 'UNI-JS-012-L', 'L', 20, 5),
(271, 160, 'UNI-JS-012-XL', 'XL', 20, 5),
(272, 160, 'UNI-JS-012-2XL', '2XL', 20, 5),
(273, 160, 'UNI-JS-012-3XL', '3XL', 20, 5),
(274, 160, 'UNI-JS-012-4XL', '4XL', 20, 5),
(275, 160, 'UNI-JS-012-5XL', '5XL', 20, 5),
(276, 160, 'UNI-JS-012-6XL', '6XL', 20, 5),
(277, 161, 'UNI-JS-013-S10', 'S10', 20, 5),
(278, 161, 'UNI-JS-013-S12', 'S12', 20, 5),
(279, 161, 'UNI-JS-013-S14', 'S14', 20, 5),
(280, 161, 'UNI-JS-013-S16', 'S16', 20, 5),
(281, 161, 'UNI-JS-013-S18', 'S18', 20, 5),
(282, 161, 'UNI-JS-013-S20', 'S20', 20, 5),
(283, 161, 'UNI-JS-013-S22', 'S22', 20, 5),
(284, 161, 'UNI-JS-013-S24', 'S24', 20, 5),
(285, 161, 'UNI-JS-013-S25', 'S25', 20, 5),
(286, 161, 'UNI-JS-013-S26', 'S26', 20, 5),
(287, 162, 'UNI-JS-014-XS', 'XS', 20, 5),
(288, 162, 'UNI-JS-014-S', 'S', 20, 5),
(289, 162, 'UNI-JS-014-M', 'M', 20, 5),
(290, 162, 'UNI-JS-014-L', 'L', 20, 5),
(291, 162, 'UNI-JS-014-XL', 'XL', 20, 5),
(292, 162, 'UNI-JS-014-2XL', '2XL', 20, 5),
(293, 162, 'UNI-JS-014-3XL', '3XL', 20, 5),
(294, 162, 'UNI-JS-014-4XL', '4XL', 20, 5),
(295, 162, 'UNI-JS-014-5XL', '5XL', 20, 5),
(296, 162, 'UNI-JS-014-6XL', '6XL', 20, 5),
(297, 163, 'UNI-JS-015-XS', 'XS', 20, 5),
(298, 163, 'UNI-JS-015-S', 'S', 20, 5),
(299, 163, 'UNI-JS-015-M', 'M', 20, 5),
(300, 163, 'UNI-JS-015-L', 'L', 20, 5),
(301, 163, 'UNI-JS-015-XL', 'XL', 20, 5),
(302, 163, 'UNI-JS-015-2XL', '2XL', 20, 5),
(303, 163, 'UNI-JS-015-3XL', '3XL', 20, 5),
(304, 163, 'UNI-JS-015-4XL', '4XL', 20, 5),
(305, 163, 'UNI-JS-015-5XL', '5XL', 20, 5),
(306, 163, 'UNI-JS-015-6XL', '6XL', 20, 5),
(307, 164, 'UNI-JS-016-XS', 'XS', 20, 5),
(308, 164, 'UNI-JS-016-S', 'S', 20, 5),
(309, 164, 'UNI-JS-016-M', 'M', 20, 5),
(310, 164, 'UNI-JS-016-L', 'L', 20, 5),
(311, 164, 'UNI-JS-016-XL', 'XL', 20, 5),
(312, 164, 'UNI-JS-016-2XL', '2XL', 20, 5),
(313, 164, 'UNI-JS-016-3XL', '3XL', 20, 5),
(314, 164, 'UNI-JS-016-4XL', '4XL', 20, 5),
(315, 164, 'UNI-JS-016-5XL', '5XL', 20, 5),
(316, 164, 'UNI-JS-016-6XL', '6XL', 20, 5),
(317, 165, 'UNI-JS-017-XS', 'XS', 20, 5),
(318, 165, 'UNI-JS-017-S', 'S', 20, 5),
(319, 165, 'UNI-JS-017-2XL', '2XL', 20, 5),
(320, 166, 'UNI-CG-018-XS', 'XS', 20, 5),
(321, 166, 'UNI-CG-018-S', 'S', 20, 5),
(322, 166, 'UNI-CG-018-M', 'M', 20, 5),
(323, 166, 'UNI-CG-018-L', 'L', 20, 5),
(324, 166, 'UNI-CG-018-XL', 'XL', 20, 5),
(325, 166, 'UNI-CG-018-2XL', '2XL', 20, 5),
(326, 166, 'UNI-CG-018-3XL', '3XL', 20, 5),
(327, 166, 'UNI-CG-018-4XL', '4XL', 20, 5),
(328, 166, 'UNI-CG-018-5XL', '5XL', 20, 5),
(329, 166, 'UNI-CG-018-6XL', '6XL', 20, 5),
(330, 166, 'UNI-CG-018-NOSIZE', 'NO SIZE', 20, 5),
(331, 167, 'UNI-CG-019-XS', 'XS', 20, 5),
(332, 167, 'UNI-CG-019-S', 'S', 20, 5),
(333, 167, 'UNI-CG-019-M', 'M', 20, 5),
(334, 167, 'UNI-CG-019-L', 'L', 20, 5),
(335, 167, 'UNI-CG-019-XL', 'XL', 20, 5),
(336, 167, 'UNI-CG-019-2XL', '2XL', 20, 5),
(337, 167, 'UNI-CG-019-3XL', '3XL', 20, 5),
(338, 167, 'UNI-CG-019-4XL', '4XL', 20, 5),
(339, 167, 'UNI-CG-019-5XL', '5XL', 20, 5),
(340, 167, 'UNI-CG-019-6XL', '6XL', 20, 5),
(341, 167, 'UNI-CG-019-NOSIZE', 'NO SIZE', 20, 5),
(342, 168, 'UNI-CG-020-XS', 'XS', 20, 5),
(343, 168, 'UNI-CG-020-S', 'S', 20, 5),
(344, 168, 'UNI-CG-020-M', 'M', 20, 5),
(345, 168, 'UNI-CG-020-L', 'L', 20, 5),
(346, 168, 'UNI-CG-020-XL', 'XL', 20, 5),
(347, 168, 'UNI-CG-020-2XL', '2XL', 20, 5),
(348, 168, 'UNI-CG-020-3XL', '3XL', 20, 5),
(349, 168, 'UNI-CG-020-4XL', '4XL', 20, 5),
(350, 168, 'UNI-CG-020-5XL', '5XL', 20, 5),
(351, 168, 'UNI-CG-020-6XL', '6XL', 20, 5),
(352, 169, 'UNI-CG-021-XS', 'XS', 20, 5),
(353, 169, 'UNI-CG-021-S', 'S', 20, 5),
(354, 169, 'UNI-CG-021-M', 'M', 20, 5),
(355, 169, 'UNI-CG-021-L', 'L', 20, 5),
(356, 169, 'UNI-CG-021-XL', 'XL', 20, 5),
(357, 169, 'UNI-CG-021-2XL', '2XL', 20, 5),
(358, 169, 'UNI-CG-021-3XL', '3XL', 20, 5),
(359, 169, 'UNI-CG-021-4XL', '4XL', 20, 5),
(360, 169, 'UNI-CG-021-5XL', '5XL', 20, 5),
(361, 169, 'UNI-CG-021-6XL', '6XL', 20, 5),
(362, 170, 'UNI-CP-022-XS', 'XS', 20, 5),
(363, 170, 'UNI-CP-022-S', 'S', 20, 5),
(364, 170, 'UNI-CP-022-M', 'M', 20, 5),
(365, 170, 'UNI-CP-022-L', 'L', 20, 5),
(366, 170, 'UNI-CP-022-XL', 'XL', 20, 5),
(367, 170, 'UNI-CP-022-2XL', '2XL', 20, 5),
(368, 170, 'UNI-CP-022-3XL', '3XL', 20, 5),
(369, 170, 'UNI-CP-022-4XL', '4XL', 20, 5),
(370, 170, 'UNI-CP-022-5XL', '5XL', 20, 5),
(371, 170, 'UNI-CP-022-6XL', '6XL', 20, 5),
(372, 171, 'UNI-CP-023-XS', 'XS', 20, 5),
(373, 171, 'UNI-CP-023-S', 'S', 20, 5),
(374, 171, 'UNI-CP-023-M', 'M', 20, 5),
(375, 171, 'UNI-CP-023-L', 'L', 20, 5),
(376, 171, 'UNI-CP-023-XL', 'XL', 20, 5),
(377, 171, 'UNI-CP-023-2XL', '2XL', 20, 5),
(378, 171, 'UNI-CP-023-3XL', '3XL', 20, 5),
(379, 171, 'UNI-CP-023-4XL', '4XL', 20, 5),
(380, 171, 'UNI-CP-023-5XL', '5XL', 20, 5),
(381, 171, 'UNI-CP-023-6XL', '6XL', 20, 5),
(382, 172, 'UNI-CP-024-XS', 'XS', 20, 5),
(383, 172, 'UNI-CP-024-S', 'S', 20, 5),
(384, 172, 'UNI-CP-024-M', 'M', 20, 5),
(385, 172, 'UNI-CP-024-L', 'L', 20, 5),
(386, 172, 'UNI-CP-024-XL', 'XL', 20, 5),
(387, 172, 'UNI-CP-024-2XL', '2XL', 20, 5),
(388, 172, 'UNI-CP-024-3XL', '3XL', 20, 5),
(389, 172, 'UNI-CP-024-4XL', '4XL', 20, 5),
(390, 172, 'UNI-CP-024-5XL', '5XL', 20, 5),
(391, 172, 'UNI-CP-024-6XL', '6XL', 20, 5),
(392, 173, 'UNI-CP-025-XS', 'XS', 20, 5),
(393, 173, 'UNI-CP-025-S', 'S', 20, 5),
(394, 173, 'UNI-CP-025-M', 'M', 20, 5),
(395, 173, 'UNI-CP-025-L', 'L', 20, 5),
(396, 173, 'UNI-CP-025-XL', 'XL', 20, 5),
(397, 173, 'UNI-CP-025-2XL', '2XL', 20, 5),
(398, 173, 'UNI-CP-025-3XL', '3XL', 20, 5),
(399, 173, 'UNI-CP-025-4XL', '4XL', 20, 5),
(400, 173, 'UNI-CP-025-5XL', '5XL', 20, 5),
(401, 173, 'UNI-CP-025-6XL', '6XL', 20, 5),
(402, 174, 'UNI-CP-026-XS', 'XS', 20, 5),
(403, 174, 'UNI-CP-026-S', 'S', 20, 5),
(404, 174, 'UNI-CP-026-M', 'M', 20, 5),
(405, 174, 'UNI-CP-026-L', 'L', 20, 5),
(406, 174, 'UNI-CP-026-XL', 'XL', 20, 5),
(407, 174, 'UNI-CP-026-2XL', '2XL', 20, 5),
(408, 174, 'UNI-CP-026-3XL', '3XL', 20, 5),
(409, 174, 'UNI-CP-026-4XL', '4XL', 20, 5),
(410, 174, 'UNI-CP-026-5XL', '5XL', 20, 5),
(411, 174, 'UNI-CP-026-6XL', '6XL', 20, 5),
(412, 175, 'UNI-CP-027-XS', 'XS', 20, 5),
(413, 175, 'UNI-CP-027-S', 'S', 20, 5),
(414, 175, 'UNI-CP-027-M', 'M', 20, 5),
(415, 175, 'UNI-CP-027-L', 'L', 20, 5),
(416, 175, 'UNI-CP-027-XL', 'XL', 20, 5),
(417, 175, 'UNI-CP-027-2XL', '2XL', 20, 5),
(418, 175, 'UNI-CP-027-3XL', '3XL', 20, 5),
(419, 175, 'UNI-CP-027-4XL', '4XL', 20, 5),
(420, 175, 'UNI-CP-027-5XL', '5XL', 20, 5),
(421, 175, 'UNI-CP-027-6XL', '6XL', 20, 5),
(422, 176, 'UNI-CP-028-S30', 'S30', 20, 5),
(423, 176, 'UNI-CP-028-S31', 'S31', 20, 5),
(424, 176, 'UNI-CP-028-S32', 'S32', 20, 5),
(425, 176, 'UNI-CP-028-S33', 'S33', 20, 5),
(426, 176, 'UNI-CP-028-S34', 'S34', 20, 5),
(427, 176, 'UNI-CP-028-S35', 'S35', 20, 5),
(428, 176, 'UNI-CP-028-S36', 'S36', 20, 5),
(429, 176, 'UNI-CP-028-S37', 'S37', 20, 5),
(430, 176, 'UNI-CP-028-S38', 'S38', 20, 5),
(431, 176, 'UNI-CP-028-S39', 'S39', 20, 5),
(432, 176, 'UNI-CP-028-S40', 'S40', 20, 5),
(433, 176, 'UNI-CP-028-S41', 'S41', 20, 5),
(434, 176, 'UNI-CP-028-S42', 'S42', 20, 5),
(435, 176, 'UNI-CP-028-S45', 'S45', 20, 5),
(436, 177, 'UNI-CP-029-XS', 'XS', 20, 5),
(437, 177, 'UNI-CP-029-S', 'S', 20, 5),
(438, 177, 'UNI-CP-029-M', 'M', 20, 5),
(439, 177, 'UNI-CP-029-L', 'L', 20, 5),
(440, 177, 'UNI-CP-029-XL', 'XL', 20, 5),
(441, 177, 'UNI-CP-029-2XL', '2XL', 20, 5),
(442, 177, 'UNI-CP-029-3XL', '3XL', 20, 5),
(443, 177, 'UNI-CP-029-4XL', '4XL', 20, 5),
(444, 177, 'UNI-CP-029-5XL', '5XL', 20, 5),
(445, 177, 'UNI-CP-029-6XL', '6XL', 20, 5),
(446, 178, 'UNI-CP-030-XS', 'XS', 20, 5),
(447, 178, 'UNI-CP-030-S', 'S', 20, 5),
(448, 178, 'UNI-CP-030-M', 'M', 20, 5),
(449, 178, 'UNI-CP-030-L', 'L', 20, 5),
(450, 178, 'UNI-CP-030-XL', 'XL', 20, 5),
(451, 178, 'UNI-CP-030-2XL', '2XL', 20, 5),
(452, 178, 'UNI-CP-030-3XL', '3XL', 20, 5),
(453, 178, 'UNI-CP-030-4XL', '4XL', 20, 5),
(454, 178, 'UNI-CP-030-5XL', '5XL', 20, 5),
(455, 178, 'UNI-CP-030-6XL', '6XL', 20, 5),
(456, 179, 'UNI-CP-031-XS', 'XS', 20, 5),
(457, 179, 'UNI-CP-031-S', 'S', 20, 5),
(458, 179, 'UNI-CP-031-M', 'M', 20, 5),
(459, 179, 'UNI-CP-031-L', 'L', 20, 5),
(460, 179, 'UNI-CP-031-XL', 'XL', 20, 5),
(461, 179, 'UNI-CP-031-2XL', '2XL', 20, 5),
(462, 179, 'UNI-CP-031-3XL', '3XL', 20, 5),
(463, 179, 'UNI-CP-031-4XL', '4XL', 20, 5),
(464, 179, 'UNI-CP-031-5XL', '5XL', 20, 5),
(465, 179, 'UNI-CP-031-6XL', '6XL', 20, 5),
(466, 180, 'UNI-CP-032-XS', 'XS', 20, 5),
(467, 180, 'UNI-CP-032-S', 'S', 20, 5),
(468, 180, 'UNI-CP-032-M', 'M', 20, 5),
(469, 180, 'UNI-CP-032-L', 'L', 20, 5),
(470, 180, 'UNI-CP-032-XL', 'XL', 20, 5),
(471, 180, 'UNI-CP-032-2XL', '2XL', 20, 5),
(472, 180, 'UNI-CP-032-3XL', '3XL', 20, 5),
(473, 180, 'UNI-CP-032-4XL', '4XL', 20, 5),
(474, 180, 'UNI-CP-032-5XL', '5XL', 20, 5),
(475, 180, 'UNI-CP-032-6XL', '6XL', 20, 5),
(476, 181, 'UNI-CP-033-XS', 'XS', 20, 5),
(477, 181, 'UNI-CP-033-S', 'S', 20, 5),
(478, 181, 'UNI-CP-033-M', 'M', 20, 5),
(479, 181, 'UNI-CP-033-L', 'L', 20, 5),
(480, 181, 'UNI-CP-033-XL', 'XL', 20, 5),
(481, 181, 'UNI-CP-033-2XL', '2XL', 20, 5),
(482, 181, 'UNI-CP-033-3XL', '3XL', 20, 5),
(483, 181, 'UNI-CP-033-4XL', '4XL', 20, 5),
(484, 181, 'UNI-CP-033-5XL', '5XL', 20, 5),
(485, 181, 'UNI-CP-033-6XL', '6XL', 20, 5),
(486, 182, 'UNI-CP-034-XS', 'XS', 20, 5),
(487, 182, 'UNI-CP-034-S', 'S', 20, 5),
(488, 182, 'UNI-CP-034-M', 'M', 20, 5),
(489, 182, 'UNI-CP-034-L', 'L', 20, 5),
(490, 182, 'UNI-CP-034-XL', 'XL', 20, 5),
(491, 182, 'UNI-CP-034-2XL', '2XL', 20, 5),
(492, 182, 'UNI-CP-034-3XL', '3XL', 20, 5),
(493, 182, 'UNI-CP-034-4XL', '4XL', 20, 5),
(494, 182, 'UNI-CP-034-5XL', '5XL', 20, 5),
(495, 182, 'UNI-CP-034-6XL', '6XL', 20, 5),
(496, 183, 'UNI-CP-035-XS', 'XS', 20, 5),
(497, 183, 'UNI-CP-035-S', 'S', 19, 5),
(498, 183, 'UNI-CP-035-M', 'M', 20, 5),
(499, 183, 'UNI-CP-035-L', 'L', 20, 5),
(500, 183, 'UNI-CP-035-XL', 'XL', 20, 5),
(501, 183, 'UNI-CP-035-2XL', '2XL', 20, 5),
(502, 183, 'UNI-CP-035-3XL', '3XL', 20, 5),
(503, 183, 'UNI-CP-035-4XL', '4XL', 20, 5),
(504, 183, 'UNI-CP-035-5XL', '5XL', 20, 5),
(505, 183, 'UNI-CP-035-6XL', '6XL', 20, 5),
(506, 184, 'UNI-CP-036-XS', 'XS', 20, 5),
(507, 184, 'UNI-CP-036-S', 'S', 20, 5),
(508, 184, 'UNI-CP-036-M', 'M', 20, 5),
(509, 184, 'UNI-CP-036-L', 'L', 20, 5),
(510, 184, 'UNI-CP-036-XL', 'XL', 20, 5),
(511, 184, 'UNI-CP-036-2XL', '2XL', 20, 5),
(512, 184, 'UNI-CP-036-3XL', '3XL', 20, 5),
(513, 184, 'UNI-CP-036-4XL', '4XL', 20, 5),
(514, 184, 'UNI-CP-036-5XL', '5XL', 20, 5),
(515, 184, 'UNI-CP-036-6XL', '6XL', 20, 5),
(516, 185, 'UNI-CP-037-XS', 'XS', 20, 5),
(517, 185, 'UNI-CP-037-S', 'S', 20, 5),
(518, 185, 'UNI-CP-037-M', 'M', 20, 5),
(519, 185, 'UNI-CP-037-L', 'L', 20, 5),
(520, 185, 'UNI-CP-037-XL', 'XL', 20, 5),
(521, 185, 'UNI-CP-037-2XL', '2XL', 20, 5),
(522, 185, 'UNI-CP-037-3XL', '3XL', 20, 5),
(523, 185, 'UNI-CP-037-4XL', '4XL', 20, 5),
(524, 185, 'UNI-CP-037-5XL', '5XL', 20, 5),
(525, 185, 'UNI-CP-037-6XL', '6XL', 20, 5),
(526, 186, 'UNI-CP-038-XS', 'XS', 20, 5),
(527, 186, 'UNI-CP-038-S', 'S', 20, 5),
(528, 186, 'UNI-CP-038-M', 'M', 20, 5),
(529, 186, 'UNI-CP-038-L', 'L', 20, 5),
(530, 186, 'UNI-CP-038-XL', 'XL', 20, 5),
(531, 186, 'UNI-CP-038-2XL', '2XL', 20, 5),
(532, 186, 'UNI-CP-038-3XL', '3XL', 20, 5),
(533, 186, 'UNI-CP-038-4XL', '4XL', 20, 5),
(534, 186, 'UNI-CP-038-5XL', '5XL', 20, 5),
(535, 186, 'UNI-CP-038-6XL', '6XL', 20, 5),
(536, 187, 'UNI-CP-039-XS', 'XS', 20, 5),
(537, 187, 'UNI-CP-039-S', 'S', 20, 5),
(538, 187, 'UNI-CP-039-M', 'M', 20, 5),
(539, 187, 'UNI-CP-039-L', 'L', 20, 5),
(540, 187, 'UNI-CP-039-XL', 'XL', 20, 5),
(541, 187, 'UNI-CP-039-2XL', '2XL', 20, 5),
(542, 187, 'UNI-CP-039-3XL', '3XL', 20, 5),
(543, 187, 'UNI-CP-039-4XL', '4XL', 20, 5),
(544, 187, 'UNI-CP-039-5XL', '5XL', 20, 5),
(545, 187, 'UNI-CP-039-6XL', '6XL', 20, 5),
(546, 188, 'UNI-CP-040-XS', 'XS', 20, 5),
(547, 188, 'UNI-CP-040-S', 'S', 20, 5),
(548, 188, 'UNI-CP-040-M', 'M', 20, 5),
(549, 188, 'UNI-CP-040-L', 'L', 20, 5),
(550, 188, 'UNI-CP-040-XL', 'XL', 20, 5),
(551, 188, 'UNI-CP-040-2XL', '2XL', 20, 5),
(552, 188, 'UNI-CP-040-3XL', '3XL', 20, 5),
(553, 188, 'UNI-CP-040-4XL', '4XL', 20, 5),
(554, 188, 'UNI-CP-040-5XL', '5XL', 20, 5),
(555, 188, 'UNI-CP-040-6XL', '6XL', 20, 5),
(556, 189, 'UNI-CP-041-XS', 'XS', 20, 5),
(557, 189, 'UNI-CP-041-S', 'S', 20, 5),
(558, 189, 'UNI-CP-041-M', 'M', 20, 5),
(559, 189, 'UNI-CP-041-L', 'L', 20, 5),
(560, 189, 'UNI-CP-041-XL', 'XL', 20, 5),
(561, 189, 'UNI-CP-041-2XL', '2XL', 20, 5),
(562, 189, 'UNI-CP-041-3XL', '3XL', 20, 5),
(563, 189, 'UNI-CP-041-4XL', '4XL', 20, 5),
(564, 189, 'UNI-CP-041-5XL', '5XL', 20, 5),
(565, 189, 'UNI-CP-041-6XL', '6XL', 20, 5),
(566, 190, 'UNI-CP-042-XS', 'XS', 20, 5),
(567, 190, 'UNI-CP-042-S', 'S', 20, 5),
(568, 190, 'UNI-CP-042-M', 'M', 20, 5),
(569, 190, 'UNI-CP-042-L', 'L', 20, 5),
(570, 190, 'UNI-CP-042-XL', 'XL', 20, 5),
(571, 190, 'UNI-CP-042-2XL', '2XL', 20, 5),
(572, 190, 'UNI-CP-042-3XL', '3XL', 20, 5),
(573, 190, 'UNI-CP-042-4XL', '4XL', 20, 5),
(574, 190, 'UNI-CP-042-5XL', '5XL', 20, 5),
(575, 190, 'UNI-CP-042-6XL', '6XL', 20, 5),
(576, 191, 'UNI-CP-043-XS', 'XS', 20, 5),
(577, 191, 'UNI-CP-043-S', 'S', 20, 5),
(578, 191, 'UNI-CP-043-M', 'M', 20, 5),
(579, 191, 'UNI-CP-043-L', 'L', 20, 5),
(580, 191, 'UNI-CP-043-XL', 'XL', 20, 5),
(581, 191, 'UNI-CP-043-2XL', '2XL', 20, 5),
(582, 191, 'UNI-CP-043-3XL', '3XL', 20, 5),
(583, 191, 'UNI-CP-043-4XL', '4XL', 20, 5),
(584, 191, 'UNI-CP-043-5XL', '5XL', 20, 5),
(585, 191, 'UNI-CP-043-6XL', '6XL', 20, 5),
(586, 192, 'UNI-CP-044-XS', 'XS', 20, 5),
(587, 192, 'UNI-CP-044-S', 'S', 20, 5),
(588, 192, 'UNI-CP-044-M', 'M', 20, 5),
(589, 192, 'UNI-CP-044-L', 'L', 20, 5),
(590, 192, 'UNI-CP-044-XL', 'XL', 20, 5),
(591, 192, 'UNI-CP-044-2XL', '2XL', 20, 5),
(592, 192, 'UNI-CP-044-3XL', '3XL', 20, 5),
(593, 192, 'UNI-CP-044-4XL', '4XL', 20, 5),
(594, 192, 'UNI-CP-044-5XL', '5XL', 20, 5),
(595, 192, 'UNI-CP-044-6XL', '6XL', 20, 5),
(596, 193, 'UNI-CP-045-XS', 'XS', 20, 5),
(597, 193, 'UNI-CP-045-S', 'S', 20, 5),
(598, 193, 'UNI-CP-045-M', 'M', 20, 5),
(599, 193, 'UNI-CP-045-L', 'L', 20, 5),
(600, 193, 'UNI-CP-045-XL', 'XL', 20, 5),
(601, 193, 'UNI-CP-045-2XL', '2XL', 20, 5),
(602, 193, 'UNI-CP-045-3XL', '3XL', 20, 5),
(603, 193, 'UNI-CP-045-4XL', '4XL', 20, 5),
(604, 193, 'UNI-CP-045-5XL', '5XL', 20, 5),
(605, 193, 'UNI-CP-045-6XL', '6XL', 20, 5),
(606, 194, 'UNI-CP-046-S30', 'S30', 20, 5),
(607, 194, 'UNI-CP-046-S31', 'S31', 20, 5),
(608, 194, 'UNI-CP-046-S32', 'S32', 20, 5),
(609, 194, 'UNI-CP-046-S33', 'S33', 20, 5),
(610, 194, 'UNI-CP-046-S34', 'S34', 20, 5),
(611, 194, 'UNI-CP-046-S35', 'S35', 20, 5),
(612, 194, 'UNI-CP-046-S36', 'S36', 20, 5),
(613, 194, 'UNI-CP-046-S37', 'S37', 20, 5),
(614, 194, 'UNI-CP-046-S38', 'S38', 20, 5),
(615, 194, 'UNI-CP-046-S39', 'S39', 20, 5),
(616, 195, 'UNI-CP-047-XS', 'XS', 20, 5),
(617, 195, 'UNI-CP-047-S', 'S', 20, 5),
(618, 195, 'UNI-CP-047-M', 'M', 20, 5),
(619, 195, 'UNI-CP-047-L', 'L', 20, 5),
(620, 195, 'UNI-CP-047-XL', 'XL', 20, 5),
(621, 195, 'UNI-CP-047-2XL', '2XL', 20, 5),
(622, 195, 'UNI-CP-047-3XL', '3XL', 20, 5),
(623, 195, 'UNI-CP-047-4XL', '4XL', 20, 5),
(624, 195, 'UNI-CP-047-5XL', '5XL', 20, 5),
(625, 195, 'UNI-CP-047-6XL', '6XL', 20, 5),
(626, 196, 'UNI-CP-048-XS', 'XS', 20, 5),
(627, 196, 'UNI-CP-048-S', 'S', 20, 5),
(628, 196, 'UNI-CP-048-M', 'M', 20, 5),
(629, 196, 'UNI-CP-048-L', 'L', 20, 5),
(630, 196, 'UNI-CP-048-XL', 'XL', 20, 5),
(631, 196, 'UNI-CP-048-2XL', '2XL', 20, 5),
(632, 196, 'UNI-CP-048-3XL', '3XL', 20, 5),
(633, 196, 'UNI-CP-048-4XL', '4XL', 20, 5),
(634, 196, 'UNI-CP-048-5XL', '5XL', 20, 5),
(635, 196, 'UNI-CP-048-6XL', '6XL', 20, 5),
(636, 197, 'UNI-CP-049-XS', 'XS', 20, 5),
(637, 197, 'UNI-CP-049-S', 'S', 20, 5),
(638, 197, 'UNI-CP-049-M', 'M', 20, 5),
(639, 197, 'UNI-CP-049-L', 'L', 20, 5),
(640, 197, 'UNI-CP-049-XL', 'XL', 20, 5),
(641, 197, 'UNI-CP-049-2XL', '2XL', 20, 5),
(642, 197, 'UNI-CP-049-3XL', '3XL', 20, 5),
(643, 197, 'UNI-CP-049-4XL', '4XL', 20, 5),
(644, 197, 'UNI-CP-049-6XL', '6XL', 20, 5),
(645, 198, 'UNI-CP-050-XS', 'XS', 20, 5),
(646, 198, 'UNI-CP-050-S', 'S', 20, 5),
(647, 198, 'UNI-CP-050-M', 'M', 20, 5),
(648, 198, 'UNI-CP-050-L', 'L', 20, 5),
(649, 198, 'UNI-CP-050-XL', 'XL', 20, 5),
(650, 198, 'UNI-CP-050-2XL', '2XL', 20, 5),
(651, 198, 'UNI-CP-050-3XL', '3XL', 20, 5),
(652, 198, 'UNI-CP-050-4XL', '4XL', 20, 5),
(653, 198, 'UNI-CP-050-5XL', '5XL', 20, 5),
(654, 198, 'UNI-CP-050-6XL', '6XL', 20, 5),
(655, 199, 'UNI-CP-051-XS', 'XS', 20, 5),
(656, 199, 'UNI-CP-051-S', 'S', 20, 5),
(657, 199, 'UNI-CP-051-M', 'M', 20, 5),
(658, 199, 'UNI-CP-051-L', 'L', 20, 5),
(659, 199, 'UNI-CP-051-XL', 'XL', 20, 5),
(660, 199, 'UNI-CP-051-2XL', '2XL', 20, 5),
(661, 199, 'UNI-CP-051-3XL', '3XL', 20, 5),
(662, 199, 'UNI-CP-051-4XL', '4XL', 20, 5),
(663, 199, 'UNI-CP-051-5XL', '5XL', 20, 5),
(664, 199, 'UNI-CP-051-6XL', '6XL', 20, 5),
(665, 200, 'UNI-CP-052-XS', 'XS', 20, 5),
(666, 200, 'UNI-CP-052-S', 'S', 20, 5),
(667, 200, 'UNI-CP-052-M', 'M', 20, 5),
(668, 200, 'UNI-CP-052-L', 'L', 20, 5),
(669, 200, 'UNI-CP-052-XL', 'XL', 20, 5),
(670, 200, 'UNI-CP-052-2XL', '2XL', 20, 5),
(671, 200, 'UNI-CP-052-3XL', '3XL', 20, 5),
(672, 200, 'UNI-CP-052-4XL', '4XL', 20, 5),
(673, 200, 'UNI-CP-052-5XL', '5XL', 20, 5),
(674, 200, 'UNI-CP-052-6XL', '6XL', 20, 5),
(675, 201, 'UNI-CP-053-XS', 'XS', 20, 5),
(676, 201, 'UNI-CP-053-S', 'S', 20, 5),
(677, 201, 'UNI-CP-053-M', 'M', 20, 5),
(678, 201, 'UNI-CP-053-L', 'L', 20, 5),
(679, 201, 'UNI-CP-053-XL', 'XL', 20, 5),
(680, 201, 'UNI-CP-053-2XL', '2XL', 20, 5),
(681, 201, 'UNI-CP-053-3XL', '3XL', 20, 5),
(682, 201, 'UNI-CP-053-4XL', '4XL', 20, 5),
(683, 201, 'UNI-CP-053-5XL', '5XL', 20, 5),
(684, 201, 'UNI-CP-053-6XL', '6XL', 20, 5),
(685, 202, 'UNI-CP-054-XS', 'XS', 20, 5),
(686, 202, 'UNI-CP-054-S', 'S', 20, 5),
(687, 202, 'UNI-CP-054-M', 'M', 20, 5),
(688, 202, 'UNI-CP-054-L', 'L', 20, 5),
(689, 202, 'UNI-CP-054-XL', 'XL', 20, 5),
(690, 202, 'UNI-CP-054-2XL', '2XL', 20, 5),
(691, 202, 'UNI-CP-054-3XL', '3XL', 20, 5),
(692, 202, 'UNI-CP-054-4XL', '4XL', 20, 5),
(693, 202, 'UNI-CP-054-5XL', '5XL', 20, 5),
(694, 202, 'UNI-CP-054-6XL', '6XL', 20, 5),
(695, 203, 'UNI-SP-055-XS', 'XS', 20, 5),
(696, 203, 'UNI-SP-055-S', 'S', 20, 5),
(697, 203, 'UNI-SP-055-M', 'M', 20, 5),
(698, 203, 'UNI-SP-055-L', 'L', 20, 5),
(699, 203, 'UNI-SP-055-XL', 'XL', 20, 5),
(700, 203, 'UNI-SP-055-2XL', '2XL', 20, 5),
(701, 203, 'UNI-SP-055-3XL', '3XL', 20, 5),
(702, 203, 'UNI-SP-055-4XL', '4XL', 20, 5),
(703, 203, 'UNI-SP-055-5XL', '5XL', 20, 5),
(704, 203, 'UNI-SP-055-6XL', '6XL', 20, 5),
(705, 204, 'UNI-SP-056-XS', 'XS', 20, 5),
(706, 204, 'UNI-SP-056-S', 'S', 20, 5),
(707, 204, 'UNI-SP-056-M', 'M', 20, 5),
(708, 204, 'UNI-SP-056-L', 'L', 20, 5),
(709, 204, 'UNI-SP-056-XL', 'XL', 20, 5),
(710, 204, 'UNI-SP-056-2XL', '2XL', 20, 5),
(711, 204, 'UNI-SP-056-3XL', '3XL', 20, 5),
(712, 204, 'UNI-SP-056-4XL', '4XL', 20, 5),
(713, 204, 'UNI-SP-056-5XL', '5XL', 20, 5),
(714, 204, 'UNI-SP-056-6XL', '6XL', 20, 5),
(715, 205, 'UNI-SP-057-XS', 'XS', 20, 5),
(716, 205, 'UNI-SP-057-S', 'S', 20, 5),
(717, 205, 'UNI-SP-057-M', 'M', 20, 5),
(718, 205, 'UNI-SP-057-L', 'L', 20, 5),
(719, 205, 'UNI-SP-057-XL', 'XL', 20, 5),
(720, 205, 'UNI-SP-057-2XL', '2XL', 20, 5),
(721, 205, 'UNI-SP-057-3XL', '3XL', 20, 5),
(722, 205, 'UNI-SP-057-4XL', '4XL', 20, 5),
(723, 205, 'UNI-SP-057-5XL', '5XL', 20, 5),
(724, 205, 'UNI-SP-057-6XL', '6XL', 20, 5),
(725, 206, 'UNI-SP-058-XS', 'XS', 20, 5),
(726, 206, 'UNI-SP-058-S', 'S', 20, 5),
(727, 206, 'UNI-SP-058-M', 'M', 20, 5),
(728, 206, 'UNI-SP-058-L', 'L', 20, 5),
(729, 206, 'UNI-SP-058-XL', 'XL', 20, 5),
(730, 206, 'UNI-SP-058-2XL', '2XL', 20, 5),
(731, 206, 'UNI-SP-058-3XL', '3XL', 20, 5),
(732, 206, 'UNI-SP-058-4XL', '4XL', 20, 5),
(733, 206, 'UNI-SP-058-5XL', '5XL', 20, 5),
(734, 206, 'UNI-SP-058-6XL', '6XL', 20, 5),
(735, 207, 'UNI-SP-059-XS', 'XS', 20, 5),
(736, 207, 'UNI-SP-059-S', 'S', 20, 5),
(737, 207, 'UNI-SP-059-M', 'M', 20, 5),
(738, 207, 'UNI-SP-059-L', 'L', 20, 5),
(739, 207, 'UNI-SP-059-XL', 'XL', 20, 5),
(740, 207, 'UNI-SP-059-2XL', '2XL', 20, 5),
(741, 207, 'UNI-SP-059-3XL', '3XL', 20, 5),
(742, 207, 'UNI-SP-059-4XL', '4XL', 20, 5),
(743, 207, 'UNI-SP-059-5XL', '5XL', 20, 5),
(744, 207, 'UNI-SP-059-6XL', '6XL', 20, 5),
(745, 208, 'UNI-ACC-001-NA', 'N/A', 30, 8),
(746, 209, 'UNI-ACC-002-NA', 'N/A', 30, 8),
(747, 210, 'UNI-ACC-003-NA', 'N/A', 30, 8),
(748, 211, 'UNI-ACC-004-NA', 'N/A', 30, 8),
(749, 212, 'UNI-ACC-005-NA', 'N/A', 30, 8),
(750, 213, 'UNI-ACC-006-NA', 'N/A', 30, 8),
(751, 214, 'UNI-ACC-007-NA', 'N/A', 30, 8),
(752, 215, 'UNI-ACC-008-NA', 'N/A', 30, 8),
(753, 216, 'UNI-ACC-009-NA', 'N/A', 30, 8),
(754, 217, 'UNI-ACC-010-NA', 'N/A', 30, 8),
(755, 218, 'UNI-ACC-011-NA', 'N/A', 30, 8),
(756, 219, 'UNI-ACC-012-NA', 'N/A', 29, 8),
(757, 220, 'UNI-ACC-013-1STYR', '1ST YR', 30, 8),
(758, 220, 'UNI-ACC-013-2NDYR', '2ND YR', 30, 8),
(759, 220, 'UNI-ACC-013-3RDYR', '3RD YR', 30, 8),
(760, 220, 'UNI-ACC-013-4THYR', '4TH YR', 30, 8),
(761, 221, 'UNI-ACC-014-1STYR', '1ST YR', 30, 8),
(762, 221, 'UNI-ACC-014-2NDYR', '2ND YR', 30, 8),
(763, 221, 'UNI-ACC-014-3RDYR', '3RD YR', 30, 8),
(764, 221, 'UNI-ACC-014-4THYR', '4TH YR', 30, 8),
(765, 222, 'UNI-ACC-015-NA', 'N/A', 30, 8),
(766, 223, 'UNI-ACC-016-NA', 'N/A', 30, 8),
(767, 224, 'UNI-ACC-017-NA', 'N/A', 30, 8),
(768, 225, 'UNI-ACC-018-NA', 'N/A', 30, 8),
(769, 226, 'UNI-ACC-019-NA', 'N/A', 30, 8),
(770, 227, 'UNI-ACC-020-NA', 'N/A', 30, 8),
(771, 228, 'BK-SAMPLE-228', 'N/A', 20, 5),
(772, 229, 'BK-SAMPLE-229', 'N/A', 20, 5),
(774, 224, 'BK-0224', 'N/A', 30, 10),
(775, 226, 'BK-0226', 'N/A', 30, 10),
(776, 230, 'BK-0230', 'N/A', 30, 10),
(777, 231, 'BK-0231', 'N/A', 30, 10),
(778, 232, 'BK-0232', 'N/A', 30, 10),
(779, 233, 'BK-0233', 'N/A', 30, 10),
(780, 234, 'BK-0234', 'N/A', 30, 10),
(781, 235, 'BK-0235', 'N/A', 30, 10),
(782, 236, 'BK-0236', 'N/A', 30, 10),
(783, 237, 'BK-0237', 'N/A', 30, 10),
(784, 238, 'BK-0238', 'N/A', 30, 10),
(785, 239, 'BK-0239', 'N/A', 30, 10),
(786, 240, 'BK-0240', 'N/A', 30, 10),
(787, 241, 'BK-0241', 'N/A', 30, 10),
(788, 242, 'BK-0242', 'N/A', 30, 10),
(789, 243, 'BK-0243', 'N/A', 30, 10),
(790, 244, 'BK-0244', 'N/A', 30, 10),
(791, 245, 'BK-0245', 'N/A', 30, 10),
(792, 246, 'BK-0246', 'N/A', 30, 10),
(793, 247, 'BK-0247', 'N/A', 30, 10),
(794, 248, 'BK-0248', 'N/A', 30, 10),
(795, 249, 'BK-0249', 'N/A', 30, 10);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_remittances`
--

CREATE TABLE `tbl_remittances` (
  `remittance_id` int(11) NOT NULL,
  `remittance_no` varchar(30) NOT NULL,
  `prepared_by` int(11) NOT NULL,
  `verified_by` int(11) DEFAULT NULL,
  `remittance_amount` decimal(10,2) NOT NULL,
  `status` enum('Pending','Verified') NOT NULL DEFAULT 'Pending',
  `prepared_at` datetime NOT NULL DEFAULT current_timestamp(),
  `verified_at` datetime DEFAULT NULL,
  `remarks` varchar(255) DEFAULT NULL,
  `or_from` varchar(30) DEFAULT NULL,
  `or_to` varchar(30) DEFAULT NULL,
  `received_by` varchar(150) DEFAULT NULL,
  `remitted_at` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_remittances`
--

INSERT INTO `tbl_remittances` (`remittance_id`, `remittance_no`, `prepared_by`, `verified_by`, `remittance_amount`, `status`, `prepared_at`, `verified_at`, `remarks`, `or_from`, `or_to`, `received_by`, `remitted_at`) VALUES
(1, 'REM-20261002211107095', 6, NULL, 25.00, 'Pending', '2026-10-02 21:11:07', NULL, 'hghehe', 'OR-20261002211016756', 'OR-20261002211016756', 'Me', '2026-10-02 21:11:07'),
(2, 'REM-20261002212859570', 2, NULL, 1100.00, 'Pending', '2026-10-02 21:28:59', NULL, '123', 'OR-20261002212507211', 'OR-20261002212507211', 'accounting', '2026-10-02 21:28:59'),
(3, 'REM-20261002220442506', 1, NULL, 1250.00, 'Pending', '2026-10-02 22:04:42', NULL, '123', 'OR-20261002205150475', 'OR-20261002205507126', '123', '2026-10-02 22:04:42');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_returns_exchanges`
--

CREATE TABLE `tbl_returns_exchanges` (
  `return_exchange_id` int(11) NOT NULL,
  `reference_no` varchar(30) NOT NULL,
  `transaction_id` int(11) NOT NULL,
  `action_type` enum('Return','Exchange') NOT NULL,
  `reason` varchar(255) NOT NULL,
  `processed_by` int(11) NOT NULL,
  `processed_at` datetime NOT NULL DEFAULT current_timestamp(),
  `status` enum('Completed','Cancelled') NOT NULL DEFAULT 'Completed'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_returns_exchanges`
--

INSERT INTO `tbl_returns_exchanges` (`return_exchange_id`, `reference_no`, `transaction_id`, `action_type`, `reason`, `processed_by`, `processed_at`, `status`) VALUES
(1, 'RET-20261002210533009', 2, 'Return', 'dsa', 1, '2026-10-02 21:05:33', 'Completed'),
(2, 'EXC-20261002212805372', 4, 'Exchange', 'wala lng', 2, '2026-10-02 21:28:05', 'Completed'),
(3, 'EXC-20261004153729154', 6, 'Exchange', '12321', 1, '2026-10-04 15:37:29', 'Completed');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_return_exchange_items`
--

CREATE TABLE `tbl_return_exchange_items` (
  `return_exchange_item_id` int(11) NOT NULL,
  `return_exchange_id` int(11) NOT NULL,
  `transaction_item_id` int(11) NOT NULL,
  `quantity` int(11) NOT NULL,
  `item_condition` varchar(50) DEFAULT NULL,
  `replacement_variant_id` int(11) DEFAULT NULL,
  `replacement_quantity` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_return_exchange_items`
--

INSERT INTO `tbl_return_exchange_items` (`return_exchange_item_id`, `return_exchange_id`, `transaction_item_id`, `quantity`, `item_condition`, `replacement_variant_id`, `replacement_quantity`) VALUES
(1, 1, 2, 4, 'das', NULL, NULL),
(2, 2, 4, 1, '3123', 756, 1),
(3, 3, 8, 1, 'Good', 497, 1);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_roles`
--

CREATE TABLE `tbl_roles` (
  `role_id` int(11) NOT NULL,
  `role_name` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_roles`
--

INSERT INTO `tbl_roles` (`role_id`, `role_name`) VALUES
(1, 'Bookstore Supervisor'),
(2, 'Cashier'),
(3, 'Inventory Staff'),
(4, 'Management');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_stock_ins`
--

CREATE TABLE `tbl_stock_ins` (
  `stock_in_id` int(11) NOT NULL,
  `reference_no` varchar(50) NOT NULL,
  `received_by` varchar(150) DEFAULT NULL,
  `stock_in_date` date NOT NULL,
  `stock_in_time` time NOT NULL,
  `created_by` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_stock_ins`
--

INSERT INTO `tbl_stock_ins` (`stock_in_id`, `reference_no`, `received_by`, `stock_in_date`, `stock_in_time`, `created_by`) VALUES
(1, 'DR-20261002210016648', 'Maria Santos', '2026-10-02', '21:00:20', 1);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_stock_in_details`
--

CREATE TABLE `tbl_stock_in_details` (
  `stock_in_detail_id` int(11) NOT NULL,
  `stock_in_id` int(11) NOT NULL,
  `variant_id` int(11) NOT NULL,
  `quantity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_stock_in_details`
--

INSERT INTO `tbl_stock_in_details` (`stock_in_detail_id`, `stock_in_id`, `variant_id`, `quantity`) VALUES
(1, 1, 1, 1);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_stock_movements`
--

CREATE TABLE `tbl_stock_movements` (
  `movement_id` int(11) NOT NULL,
  `variant_id` int(11) NOT NULL,
  `movement_type` enum('Stock In','Stock Out','Damaged','Missing','Returned','Adjustment') NOT NULL,
  `quantity` int(11) NOT NULL,
  `previous_quantity` int(11) NOT NULL,
  `new_quantity` int(11) NOT NULL,
  `reference_no` varchar(100) DEFAULT NULL,
  `remarks` varchar(255) DEFAULT NULL,
  `created_by` int(11) NOT NULL,
  `created_at` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_stock_movements`
--

INSERT INTO `tbl_stock_movements` (`movement_id`, `variant_id`, `movement_type`, `quantity`, `previous_quantity`, `new_quantity`, `reference_no`, `remarks`, `created_by`, `created_at`) VALUES
(1, 1, 'Stock In', 1, 100, 101, 'DR-20261002210016648', 'Stock received', 1, '2026-10-02 21:00:20'),
(2, 1, 'Adjustment', 91, 101, 10, 'CNT-20261002210232', 'e', 1, '2026-10-02 21:02:59'),
(3, 7, 'Returned', 4, 0, 4, 'RET-20261002210533009', 'Return of TXN-20261002205439014', 1, '2026-10-02 21:05:33'),
(4, 665, 'Returned', 1, 19, 20, 'EXC-20261002212805372', 'Exchange of TXN-20261002211935813', 2, '2026-10-02 21:28:05'),
(5, 756, 'Stock Out', 1, 30, 29, 'EXC-20261002212805372', 'Exchange replacement for TXN-20261002211935813', 2, '2026-10-02 21:28:05'),
(6, 1, 'Adjustment', 5, 10, 5, 'CNT-20261002213112', '231231', 1, '2026-10-02 21:31:54'),
(7, 17, 'Returned', 1, 99, 100, 'EXC-20261004153729154', 'Exchange of TXN-20261004152552471', 1, '2026-10-04 15:37:29'),
(8, 497, 'Stock Out', 1, 20, 19, 'EXC-20261004153729154', 'Exchange replacement for TXN-20261004152552471', 1, '2026-10-04 15:37:29');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_students`
--

CREATE TABLE `tbl_students` (
  `student_id` int(11) NOT NULL,
  `student_no` varchar(20) NOT NULL,
  `last_name` varchar(100) NOT NULL,
  `first_name` varchar(100) NOT NULL,
  `education_level` varchar(50) DEFAULT NULL,
  `grade_level` varchar(30) NOT NULL,
  `program_strand` varchar(100) DEFAULT NULL,
  `section` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_students`
--

INSERT INTO `tbl_students` (`student_id`, `student_no`, `last_name`, `first_name`, `education_level`, `grade_level`, `program_strand`, `section`) VALUES
(1, '2235-20', 'Dela Cruz', 'Miguel', 'Grade School', 'Grade 3', NULL, 'Sampaguita'),
(2, '2346-24', 'Villanueva', 'Isabella', 'Grade School', 'Grade 5', '', 'Rosal'),
(3, '1543-23', 'Ramirez', 'Gabriel', 'Grade School', 'Grade 7', '', 'Narra'),
(4, '1235-23', 'Aquino', 'Sofia', 'Grade School', 'Grade 8', '', 'Molave'),
(5, '2435-23', 'Castillo', 'Lucas', 'Grade School', 'Grade 10', NULL, 'Newton'),
(6, '2324-24', 'Mercado', 'Angela', 'Grade School', 'Grade 10', '', 'Einstein'),
(7, '2864-23', 'Navarro', 'Joshua', 'Senior High School', 'Grade 11', 'ABM', 'STEM-A'),
(8, '1234-22', 'Pascual', 'Bianca', 'Senior High School', 'Grade 12', 'HUMSS', 'ABM-B'),
(9, '2343-22', 'Fernandez', 'Carlo', 'College', '1st Year College', 'BSCS', 'BSIT 1A'),
(10, '1785-23', 'Domingo', 'Patricia', 'College', '2nd Year College', 'BSCpE', 'BSCS-2B'),
(11, '1123-24', 'Fernandez', 'Gio', 'College', '3rd Year', 'BSPsych', '31E1'),
(12, '1127-24', 'Enclona', 'Paul Benedict', 'College', '3rd Year', 'BSBA', '31E1'),
(13, '1208-24', 'Para', 'Andrea', 'College', '3rd Year', 'BSBA', '31E1'),
(14, '1314-24', 'Batoy', 'Nicholo John', 'College', '3rd Year', 'BSCS', '31E1'),
(15, '1327-24', 'Reales', 'Jonnidel', 'College', '3rd Year', 'JD', '31E1'),
(16, '1395-24', 'Solis', 'Sophia Cassandra', 'College', '3rd Year', 'BSREM', '31E3'),
(17, '1396-24', 'Mendoza', 'Stephanie', 'College', '3rd Year', 'BSPsych', '31E1'),
(18, '1522-24', 'Barcinas', 'Marc Denize', 'College', '3rd Year', 'BSHM', '31E1'),
(19, '1808-23', 'Villacorte', 'Joshua', 'College', '3rd Year', 'BSCA', '31E1'),
(20, '2055-24', 'Canua', 'Carl James', 'College', '3rd Year', 'BSIT', '31E3'),
(21, '2056-24', 'Ramones', 'Leisbeth', 'College', '3rd Year', 'BSIT', '31E1'),
(22, '2154-24', 'Sabasaje', 'Sho Uno', 'College', '3rd Year', 'BSHM', '31E1'),
(23, '2208-24', 'Eullo', 'John Raven', 'College', '3rd Year', 'BSCS', '31E1'),
(24, '2786-24', 'Roque', 'Kevin Clerck', 'College', '3rd Year', 'BSIE', '31E1'),
(25, '2789-24', 'De Vera', 'Alliyah', 'College', '3rd Year', 'BSCpE', '31E1'),
(26, '2657-24', 'Mikhailovna', 'Alyah', NULL, '3rd Year College', 'BSIT', '31E1');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_transactions`
--

CREATE TABLE `tbl_transactions` (
  `transaction_id` int(11) NOT NULL,
  `transaction_no` varchar(30) NOT NULL,
  `buyer_type` varchar(20) NOT NULL,
  `student_id` int(11) DEFAULT NULL,
  `buyer_name` varchar(150) NOT NULL,
  `or_no` varchar(30) NOT NULL,
  `or_date` date NOT NULL,
  `created_at` datetime NOT NULL DEFAULT current_timestamp(),
  `payment_method` varchar(30) NOT NULL DEFAULT 'Cash',
  `employee_name` varchar(100) DEFAULT NULL,
  `total_amount` decimal(10,2) NOT NULL,
  `amount_paid` decimal(10,2) NOT NULL DEFAULT 0.00,
  `amount_change` decimal(10,2) NOT NULL DEFAULT 0.00,
  `created_by` int(11) NOT NULL,
  `status` varchar(20) NOT NULL DEFAULT 'Completed',
  `cancel_reason` varchar(255) DEFAULT NULL,
  `cancelled_by` int(11) DEFAULT NULL,
  `cancelled_at` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_transactions`
--

INSERT INTO `tbl_transactions` (`transaction_id`, `transaction_no`, `buyer_type`, `student_id`, `buyer_name`, `or_no`, `or_date`, `created_at`, `payment_method`, `employee_name`, `total_amount`, `amount_paid`, `amount_change`, `created_by`, `status`, `cancel_reason`, `cancelled_by`, `cancelled_at`) VALUES
(1, 'TXN-20261002205141910', 'Student', 25, 'Alliyah De Vera', 'OR-20261002205150475', '2026-10-02', '2026-10-02 20:54:02', 'Cash', NULL, 25.00, 30.00, 5.00, 1, 'Completed', NULL, NULL, NULL),
(2, 'TXN-20261002205439014', 'Walk-in', NULL, '123', 'OR-20261002205507126', '2026-10-02', '2026-10-02 20:55:14', 'Cash', NULL, 1225.00, 3000.00, 1775.00, 1, 'Partially Returned', NULL, NULL, NULL),
(3, 'TXN-20261002210939244', 'Student', 1, 'Miguel Dela Cruz', 'OR-20261002211016756', '2026-10-02', '2026-10-02 21:10:27', 'Salary Deduction', 'Daniel Lopez', 25.00, 25.00, 0.00, 6, 'Completed', NULL, NULL, NULL),
(4, 'TXN-20261002211935813', 'Student', 25, 'Alliyah De Vera', 'OR-20261002212507211', '2026-10-02', '2026-10-02 21:25:25', 'Cash', NULL, 850.00, 900.00, 50.00, 2, 'Exchanged', NULL, NULL, NULL),
(5, 'TXN-20261004150727709', 'Student', 18, 'Marc Denize Barcinas', 'OR-20261004150738291', '2026-10-04', '2026-10-04 15:08:02', 'Cash', NULL, 25.00, 30.00, 5.00, 1, 'Completed', NULL, NULL, NULL),
(6, 'TXN-20261004152552471', 'Student', 17, 'Stephanie Mendoza', 'OR-20261004152656588', '2026-10-04', '2026-10-04 15:27:33', 'Cash', NULL, 275.00, 300.00, 25.00, 1, 'Partially Exchanged', NULL, NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_transaction_items`
--

CREATE TABLE `tbl_transaction_items` (
  `transaction_item_id` int(11) NOT NULL,
  `transaction_id` int(11) NOT NULL,
  `variant_id` int(11) NOT NULL,
  `quantity` int(11) NOT NULL DEFAULT 1,
  `subtotal` decimal(10,2) NOT NULL,
  `is_backorder` tinyint(1) NOT NULL DEFAULT 0,
  `pickup_date` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_transaction_items`
--

INSERT INTO `tbl_transaction_items` (`transaction_item_id`, `transaction_id`, `variant_id`, `quantity`, `subtotal`, `is_backorder`, `pickup_date`) VALUES
(1, 1, 7, 1, 25.00, 0, NULL),
(2, 2, 7, 49, 1225.00, 0, NULL),
(3, 3, 5, 1, 25.00, 0, NULL),
(4, 4, 665, 1, 850.00, 0, NULL),
(5, 5, 7, 1, 25.00, 0, NULL),
(6, 6, 3, 1, 25.00, 0, NULL),
(7, 6, 11, 1, 30.00, 0, NULL),
(8, 6, 17, 1, 220.00, 0, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_users`
--

CREATE TABLE `tbl_users` (
  `user_id` int(11) NOT NULL,
  `username` varchar(50) NOT NULL,
  `password` varchar(255) NOT NULL,
  `first_name` varchar(100) NOT NULL,
  `last_name` varchar(100) NOT NULL,
  `role_id` int(11) NOT NULL,
  `status` varchar(20) NOT NULL DEFAULT 'Active'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_users`
--

INSERT INTO `tbl_users` (`user_id`, `username`, `password`, `first_name`, `last_name`, `role_id`, `status`) VALUES
(1, 'msantos', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Maria', 'Santos', 1, 'Active'),
(2, 'jreyes', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Jose', 'Reyes', 2, 'Active'),
(3, 'acruz', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Ana', 'Cruz', 2, 'Active'),
(4, 'rdelacruz', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Ramon', 'Dela Cruz', 3, 'Active'),
(5, 'lgarcia', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Luz', 'Garcia', 3, 'Active'),
(6, 'pbautista', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Paolo', 'Bautista', 2, 'Active'),
(7, 'cmendoza', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Carla', 'Mendoza', 4, 'Active'),
(8, 'ftorres', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Fernando', 'Torres', 4, 'Active'),
(9, 'hramos', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Hazel', 'Ramos', 1, 'Active'),
(10, 'dlopez', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Daniel', 'Lopez', 3, 'Active');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tbl_audit_logs`
--
ALTER TABLE `tbl_audit_logs`
  ADD PRIMARY KEY (`audit_id`),
  ADD KEY `fk_audit_user` (`user_id`);

--
-- Indexes for table `tbl_cash_denominations`
--
ALTER TABLE `tbl_cash_denominations`
  ADD PRIMARY KEY (`denomination_id`);

--
-- Indexes for table `tbl_categories`
--
ALTER TABLE `tbl_categories`
  ADD PRIMARY KEY (`category_id`),
  ADD UNIQUE KEY `category_name` (`category_name`);

--
-- Indexes for table `tbl_category_types`
--
ALTER TABLE `tbl_category_types`
  ADD PRIMARY KEY (`category_type_id`),
  ADD KEY `fk_type_category` (`category_id`);

--
-- Indexes for table `tbl_employees`
--
ALTER TABLE `tbl_employees`
  ADD PRIMARY KEY (`employee_id`),
  ADD UNIQUE KEY `uq_employee_no` (`employee_no`);

--
-- Indexes for table `tbl_end_of_day`
--
ALTER TABLE `tbl_end_of_day`
  ADD PRIMARY KEY (`end_of_day_id`),
  ADD UNIQUE KEY `reconciliation_no` (`reconciliation_no`),
  ADD KEY `fk_eod_cashier` (`cashier_id`);

--
-- Indexes for table `tbl_inventory_counts`
--
ALTER TABLE `tbl_inventory_counts`
  ADD PRIMARY KEY (`inventory_count_id`),
  ADD UNIQUE KEY `count_no` (`count_no`),
  ADD KEY `fk_inventory_count_user` (`prepared_by`);

--
-- Indexes for table `tbl_inventory_count_details`
--
ALTER TABLE `tbl_inventory_count_details`
  ADD PRIMARY KEY (`inventory_count_detail_id`),
  ADD KEY `fk_count_detail_header` (`inventory_count_id`),
  ADD KEY `fk_count_detail_variant` (`variant_id`);

--
-- Indexes for table `tbl_products`
--
ALTER TABLE `tbl_products`
  ADD PRIMARY KEY (`product_id`),
  ADD KEY `fk_product_type` (`category_type_id`);

--
-- Indexes for table `tbl_product_variants`
--
ALTER TABLE `tbl_product_variants`
  ADD PRIMARY KEY (`variant_id`),
  ADD UNIQUE KEY `product_code` (`product_code`),
  ADD KEY `fk_variant_product` (`product_id`);

--
-- Indexes for table `tbl_remittances`
--
ALTER TABLE `tbl_remittances`
  ADD PRIMARY KEY (`remittance_id`),
  ADD UNIQUE KEY `remittance_no` (`remittance_no`),
  ADD KEY `fk_remittance_prepared` (`prepared_by`),
  ADD KEY `fk_remittance_verified` (`verified_by`);

--
-- Indexes for table `tbl_returns_exchanges`
--
ALTER TABLE `tbl_returns_exchanges`
  ADD PRIMARY KEY (`return_exchange_id`),
  ADD UNIQUE KEY `reference_no` (`reference_no`),
  ADD KEY `fk_re_transaction` (`transaction_id`),
  ADD KEY `fk_re_user` (`processed_by`);

--
-- Indexes for table `tbl_return_exchange_items`
--
ALTER TABLE `tbl_return_exchange_items`
  ADD PRIMARY KEY (`return_exchange_item_id`),
  ADD KEY `fk_rei_header` (`return_exchange_id`),
  ADD KEY `fk_rei_transaction_item` (`transaction_item_id`),
  ADD KEY `fk_rei_replacement` (`replacement_variant_id`);

--
-- Indexes for table `tbl_roles`
--
ALTER TABLE `tbl_roles`
  ADD PRIMARY KEY (`role_id`),
  ADD UNIQUE KEY `role_name` (`role_name`);

--
-- Indexes for table `tbl_stock_ins`
--
ALTER TABLE `tbl_stock_ins`
  ADD PRIMARY KEY (`stock_in_id`),
  ADD UNIQUE KEY `reference_no` (`reference_no`),
  ADD KEY `fk_stockin_user` (`created_by`);

--
-- Indexes for table `tbl_stock_in_details`
--
ALTER TABLE `tbl_stock_in_details`
  ADD PRIMARY KEY (`stock_in_detail_id`),
  ADD KEY `fk_sid_stockin` (`stock_in_id`),
  ADD KEY `fk_sid_variant` (`variant_id`);

--
-- Indexes for table `tbl_stock_movements`
--
ALTER TABLE `tbl_stock_movements`
  ADD PRIMARY KEY (`movement_id`),
  ADD KEY `fk_movement_variant` (`variant_id`),
  ADD KEY `fk_movement_user` (`created_by`);

--
-- Indexes for table `tbl_students`
--
ALTER TABLE `tbl_students`
  ADD PRIMARY KEY (`student_id`),
  ADD UNIQUE KEY `student_no` (`student_no`);

--
-- Indexes for table `tbl_transactions`
--
ALTER TABLE `tbl_transactions`
  ADD PRIMARY KEY (`transaction_id`),
  ADD UNIQUE KEY `transaction_no` (`transaction_no`),
  ADD UNIQUE KEY `or_no` (`or_no`),
  ADD KEY `fk_txn_student` (`student_id`),
  ADD KEY `fk_txn_user` (`created_by`);

--
-- Indexes for table `tbl_transaction_items`
--
ALTER TABLE `tbl_transaction_items`
  ADD PRIMARY KEY (`transaction_item_id`),
  ADD KEY `fk_ti_txn` (`transaction_id`),
  ADD KEY `fk_ti_variant` (`variant_id`);

--
-- Indexes for table `tbl_users`
--
ALTER TABLE `tbl_users`
  ADD PRIMARY KEY (`user_id`),
  ADD UNIQUE KEY `username` (`username`),
  ADD KEY `fk_user_role` (`role_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `tbl_audit_logs`
--
ALTER TABLE `tbl_audit_logs`
  MODIFY `audit_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=97;

--
-- AUTO_INCREMENT for table `tbl_cash_denominations`
--
ALTER TABLE `tbl_cash_denominations`
  MODIFY `denomination_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_categories`
--
ALTER TABLE `tbl_categories`
  MODIFY `category_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `tbl_category_types`
--
ALTER TABLE `tbl_category_types`
  MODIFY `category_type_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=41;

--
-- AUTO_INCREMENT for table `tbl_employees`
--
ALTER TABLE `tbl_employees`
  MODIFY `employee_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `tbl_end_of_day`
--
ALTER TABLE `tbl_end_of_day`
  MODIFY `end_of_day_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `tbl_inventory_counts`
--
ALTER TABLE `tbl_inventory_counts`
  MODIFY `inventory_count_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `tbl_inventory_count_details`
--
ALTER TABLE `tbl_inventory_count_details`
  MODIFY `inventory_count_detail_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `tbl_products`
--
ALTER TABLE `tbl_products`
  MODIFY `product_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=250;

--
-- AUTO_INCREMENT for table `tbl_product_variants`
--
ALTER TABLE `tbl_product_variants`
  MODIFY `variant_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=805;

--
-- AUTO_INCREMENT for table `tbl_remittances`
--
ALTER TABLE `tbl_remittances`
  MODIFY `remittance_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `tbl_returns_exchanges`
--
ALTER TABLE `tbl_returns_exchanges`
  MODIFY `return_exchange_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `tbl_return_exchange_items`
--
ALTER TABLE `tbl_return_exchange_items`
  MODIFY `return_exchange_item_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `tbl_roles`
--
ALTER TABLE `tbl_roles`
  MODIFY `role_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `tbl_stock_ins`
--
ALTER TABLE `tbl_stock_ins`
  MODIFY `stock_in_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `tbl_stock_in_details`
--
ALTER TABLE `tbl_stock_in_details`
  MODIFY `stock_in_detail_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `tbl_stock_movements`
--
ALTER TABLE `tbl_stock_movements`
  MODIFY `movement_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `tbl_students`
--
ALTER TABLE `tbl_students`
  MODIFY `student_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=27;

--
-- AUTO_INCREMENT for table `tbl_transactions`
--
ALTER TABLE `tbl_transactions`
  MODIFY `transaction_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `tbl_transaction_items`
--
ALTER TABLE `tbl_transaction_items`
  MODIFY `transaction_item_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `tbl_users`
--
ALTER TABLE `tbl_users`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `tbl_audit_logs`
--
ALTER TABLE `tbl_audit_logs`
  ADD CONSTRAINT `fk_audit_user` FOREIGN KEY (`user_id`) REFERENCES `tbl_users` (`user_id`);

--
-- Constraints for table `tbl_category_types`
--
ALTER TABLE `tbl_category_types`
  ADD CONSTRAINT `fk_type_category` FOREIGN KEY (`category_id`) REFERENCES `tbl_categories` (`category_id`);

--
-- Constraints for table `tbl_end_of_day`
--
ALTER TABLE `tbl_end_of_day`
  ADD CONSTRAINT `fk_eod_cashier` FOREIGN KEY (`cashier_id`) REFERENCES `tbl_users` (`user_id`);

--
-- Constraints for table `tbl_inventory_counts`
--
ALTER TABLE `tbl_inventory_counts`
  ADD CONSTRAINT `fk_inventory_count_user` FOREIGN KEY (`prepared_by`) REFERENCES `tbl_users` (`user_id`);

--
-- Constraints for table `tbl_inventory_count_details`
--
ALTER TABLE `tbl_inventory_count_details`
  ADD CONSTRAINT `fk_count_detail_header` FOREIGN KEY (`inventory_count_id`) REFERENCES `tbl_inventory_counts` (`inventory_count_id`),
  ADD CONSTRAINT `fk_count_detail_variant` FOREIGN KEY (`variant_id`) REFERENCES `tbl_product_variants` (`variant_id`);

--
-- Constraints for table `tbl_products`
--
ALTER TABLE `tbl_products`
  ADD CONSTRAINT `fk_product_type` FOREIGN KEY (`category_type_id`) REFERENCES `tbl_category_types` (`category_type_id`);

--
-- Constraints for table `tbl_product_variants`
--
ALTER TABLE `tbl_product_variants`
  ADD CONSTRAINT `fk_variant_product` FOREIGN KEY (`product_id`) REFERENCES `tbl_products` (`product_id`);

--
-- Constraints for table `tbl_remittances`
--
ALTER TABLE `tbl_remittances`
  ADD CONSTRAINT `fk_remittance_prepared` FOREIGN KEY (`prepared_by`) REFERENCES `tbl_users` (`user_id`),
  ADD CONSTRAINT `fk_remittance_verified` FOREIGN KEY (`verified_by`) REFERENCES `tbl_users` (`user_id`);

--
-- Constraints for table `tbl_returns_exchanges`
--
ALTER TABLE `tbl_returns_exchanges`
  ADD CONSTRAINT `fk_re_transaction` FOREIGN KEY (`transaction_id`) REFERENCES `tbl_transactions` (`transaction_id`),
  ADD CONSTRAINT `fk_re_user` FOREIGN KEY (`processed_by`) REFERENCES `tbl_users` (`user_id`);

--
-- Constraints for table `tbl_return_exchange_items`
--
ALTER TABLE `tbl_return_exchange_items`
  ADD CONSTRAINT `fk_rei_header` FOREIGN KEY (`return_exchange_id`) REFERENCES `tbl_returns_exchanges` (`return_exchange_id`),
  ADD CONSTRAINT `fk_rei_replacement` FOREIGN KEY (`replacement_variant_id`) REFERENCES `tbl_product_variants` (`variant_id`),
  ADD CONSTRAINT `fk_rei_transaction_item` FOREIGN KEY (`transaction_item_id`) REFERENCES `tbl_transaction_items` (`transaction_item_id`);

--
-- Constraints for table `tbl_stock_ins`
--
ALTER TABLE `tbl_stock_ins`
  ADD CONSTRAINT `fk_stockin_user` FOREIGN KEY (`created_by`) REFERENCES `tbl_users` (`user_id`);

--
-- Constraints for table `tbl_stock_in_details`
--
ALTER TABLE `tbl_stock_in_details`
  ADD CONSTRAINT `fk_sid_stockin` FOREIGN KEY (`stock_in_id`) REFERENCES `tbl_stock_ins` (`stock_in_id`),
  ADD CONSTRAINT `fk_sid_variant` FOREIGN KEY (`variant_id`) REFERENCES `tbl_product_variants` (`variant_id`);

--
-- Constraints for table `tbl_stock_movements`
--
ALTER TABLE `tbl_stock_movements`
  ADD CONSTRAINT `fk_movement_user` FOREIGN KEY (`created_by`) REFERENCES `tbl_users` (`user_id`),
  ADD CONSTRAINT `fk_movement_variant` FOREIGN KEY (`variant_id`) REFERENCES `tbl_product_variants` (`variant_id`);

--
-- Constraints for table `tbl_transactions`
--
ALTER TABLE `tbl_transactions`
  ADD CONSTRAINT `fk_txn_student` FOREIGN KEY (`student_id`) REFERENCES `tbl_students` (`student_id`),
  ADD CONSTRAINT `fk_txn_user` FOREIGN KEY (`created_by`) REFERENCES `tbl_users` (`user_id`);

--
-- Constraints for table `tbl_transaction_items`
--
ALTER TABLE `tbl_transaction_items`
  ADD CONSTRAINT `fk_ti_txn` FOREIGN KEY (`transaction_id`) REFERENCES `tbl_transactions` (`transaction_id`),
  ADD CONSTRAINT `fk_ti_variant` FOREIGN KEY (`variant_id`) REFERENCES `tbl_product_variants` (`variant_id`);

--
-- Constraints for table `tbl_users`
--
ALTER TABLE `tbl_users`
  ADD CONSTRAINT `fk_user_role` FOREIGN KEY (`role_id`) REFERENCES `tbl_roles` (`role_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
