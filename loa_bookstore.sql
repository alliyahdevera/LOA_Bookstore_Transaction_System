-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Sep 24, 2026 at 02:55 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.0.30

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
(10, 5, 'Filing and Folders');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_products`
--

CREATE TABLE `tbl_products` (
  `product_id` int(11) NOT NULL,
  `product_name` varchar(150) NOT NULL,
  `product_description` varchar(255) DEFAULT NULL,
  `category_type_id` int(11) NOT NULL,
  `unit_price` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_products`
--

INSERT INTO `tbl_products` (`product_id`, `product_name`, `product_description`, `category_type_id`, `unit_price`) VALUES
(1, 'Elementary Boys Polo Shirt', 'Official LOA Kinder to Grade 6 boys polo', 1, 285.00),
(2, 'Junior High Girls Blouse', 'Official LOA Grade 7-10 girls blouse', 2, 310.00),
(3, 'Senior High Polo Shirt', 'Official LOA Grade 11-12 polo shirt', 3, 340.00),
(4, 'College Polo Shirt', 'Official LOA college polo shirt', 4, 380.00),
(5, 'PE T-Shirt', 'Official LOA physical education shirt', 5, 250.00),
(6, 'Grade 7 English Textbook', 'English textbook for Grade 7', 6, 450.00),
(7, 'Grade 10 Math Learning Module', 'Mathematics learning module for Grade 10', 7, 180.00),
(8, 'Ballpen (Blue)', 'Blue ink ballpen, 0.7mm', 8, 12.00),
(9, 'Notebook 80 Leaves', 'Spiral notebook, 80 leaves', 9, 35.00),
(10, 'Expanding Folder (Long)', 'Long plastic expanding folder', 10, 65.00);

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
(1, 1, 'UNI-ELB-08', '8', 47, 10),
(2, 1, 'UNI-ELB-10', '10', 40, 10),
(3, 2, 'UNI-JHG-S', 'S', 38, 10),
(4, 2, 'UNI-JHG-M', 'M', 8, 10),
(5, 3, 'UNI-SHP-M', 'M', 33, 10),
(6, 3, 'UNI-SHP-L', 'L', 29, 8),
(7, 4, 'UNI-COL-M', 'M', 30, 8),
(8, 4, 'UNI-COL-L', 'L', 28, 8),
(9, 5, 'UNI-PE-S', 'S', 57, 15),
(10, 5, 'UNI-PE-M', 'M', 58, 15),
(11, 6, 'BK-ENG7', 'N/A', 79, 15),
(12, 7, 'MOD-MATH10', 'N/A', 118, 20),
(13, 8, 'SUP-BP-BLU', 'N/A', 462, 50),
(14, 9, 'SUP-NB-80', 'N/A', 241, 40),
(15, 10, 'OFF-FLD-EXP', 'N/A', 33, 15);

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
  `stock_in_date` date NOT NULL,
  `stock_in_time` time NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_stock_ins`
--

INSERT INTO `tbl_stock_ins` (`stock_in_id`, `reference_no`, `stock_in_date`, `stock_in_time`) VALUES
(1, 'DR-2026-001', '2026-06-15', '09:30:00'),
(2, 'DR-2026-002', '2026-06-15', '14:10:00'),
(3, 'DR-2026-003', '2026-06-22', '10:00:00'),
(4, 'DR-2026-004', '2026-06-22', '13:45:00'),
(5, 'DR-2026-005', '2026-07-06', '09:15:00'),
(6, 'DR-2026-006', '2026-07-13', '11:20:00'),
(7, 'DR-2026-007', '2026-07-13', '14:30:00'),
(8, 'DR-2026-008', '2026-07-20', '10:05:00'),
(9, 'DR-2026-009', '2026-07-20', '10:40:00'),
(10, 'DR-2026-010', '2026-08-03', '15:00:00');

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
(1, 1, 1, 50),
(2, 1, 2, 40),
(3, 2, 3, 40),
(4, 2, 4, 30),
(5, 3, 5, 35),
(6, 3, 6, 30),
(7, 4, 7, 30),
(8, 4, 8, 30),
(9, 5, 9, 60),
(10, 5, 10, 60),
(11, 6, 11, 80),
(12, 7, 12, 120),
(13, 8, 13, 500),
(14, 9, 14, 250),
(15, 10, 15, 40);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_students`
--

CREATE TABLE `tbl_students` (
  `student_id` int(11) NOT NULL,
  `student_no` varchar(20) NOT NULL,
  `last_name` varchar(100) NOT NULL,
  `first_name` varchar(100) NOT NULL,
  `grade_level` varchar(30) NOT NULL,
  `section` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_students`
--

INSERT INTO `tbl_students` (`student_id`, `student_no`, `last_name`, `first_name`, `grade_level`, `section`) VALUES
(1, '2026-00101', 'Dela Cruz', 'Miguel', 'Grade 3', 'Sampaguita'),
(2, '2026-00102', 'Villanueva', 'Isabella', 'Grade 5', 'Rosal'),
(3, '2025-00215', 'Ramirez', 'Gabriel', 'Grade 7', 'Narra'),
(4, '2025-00216', 'Aquino', 'Sofia', 'Grade 8', 'Molave'),
(5, '2024-00317', 'Castillo', 'Lucas', 'Grade 10', 'Newton'),
(6, '2024-00318', 'Mercado', 'Angela', 'Grade 10', 'Einstein'),
(7, '2025-00420', 'Navarro', 'Joshua', 'Grade 11', 'STEM-A'),
(8, '2025-00421', 'Pascual', 'Bianca', 'Grade 12', 'ABM-B'),
(9, '2026-00530', 'Fernandez', 'Carlo', '1st Year College', 'BSIT-1A'),
(10, '2026-00531', 'Domingo', 'Patricia', '2nd Year College', 'BSCS-2B');

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
  `total_amount` decimal(10,2) NOT NULL,
  `created_by` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_transactions`
--

INSERT INTO `tbl_transactions` (`transaction_id`, `transaction_no`, `buyer_type`, `student_id`, `buyer_name`, `or_no`, `or_date`, `total_amount`, `created_by`) VALUES
(1, 'TXN-20260817-001', 'Student', 1, 'Miguel Dela Cruz', 'OR-000451', '2026-08-17', 820.00, 2),
(2, 'TXN-20260817-002', 'Student', 2, 'Isabella Villanueva', 'OR-000452', '2026-08-17', 605.00, 3),
(3, 'TXN-20260818-001', 'Student', 3, 'Gabriel Ramirez', 'OR-000453', '2026-08-18', 580.00, 2),
(4, 'TXN-20260818-002', 'Student', 4, 'Sofia Aquino', 'OR-000454', '2026-08-18', 870.00, 6),
(5, 'TXN-20260819-001', 'Student', 5, 'Lucas Castillo', 'OR-000455', '2026-08-19', 216.00, 3),
(6, 'TXN-20260820-001', 'Student', 7, 'Joshua Navarro', 'OR-000456', '2026-08-20', 680.00, 2),
(7, 'TXN-20260821-001', 'Student', 8, 'Bianca Pascual', 'OR-000457', '2026-08-21', 590.00, 6),
(8, 'TXN-20260824-001', 'Student', 9, 'Carlo Fernandez', 'OR-000458', '2026-08-24', 890.00, 3),
(9, 'TXN-20260901-001', 'Employee', NULL, 'Teresita Manalo', 'OR-000459', '2026-09-01', 455.00, 2),
(10, 'TXN-20260902-001', 'Parent', 6, 'Ligaya Mercado', 'OR-000460', '2026-09-02', 800.00, 2);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_transaction_items`
--

CREATE TABLE `tbl_transaction_items` (
  `transaction_item_id` int(11) NOT NULL,
  `transaction_id` int(11) NOT NULL,
  `variant_id` int(11) NOT NULL,
  `subtotal` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_transaction_items`
--

INSERT INTO `tbl_transaction_items` (`transaction_item_id`, `transaction_id`, `variant_id`, `subtotal`) VALUES
(1, 1, 1, 570.00),
(2, 1, 9, 250.00),
(3, 2, 9, 500.00),
(4, 2, 14, 105.00),
(5, 3, 11, 450.00),
(6, 3, 13, 60.00),
(7, 3, 14, 70.00),
(8, 4, 3, 620.00),
(9, 4, 10, 250.00),
(10, 5, 12, 180.00),
(11, 5, 13, 36.00),
(12, 6, 5, 680.00),
(13, 7, 6, 340.00),
(14, 7, 10, 250.00),
(15, 8, 8, 760.00),
(16, 8, 15, 130.00),
(17, 9, 14, 140.00),
(18, 9, 15, 195.00),
(19, 9, 13, 120.00),
(20, 10, 4, 620.00),
(21, 10, 12, 180.00);

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
  `role_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_users`
--

INSERT INTO `tbl_users` (`user_id`, `username`, `password`, `first_name`, `last_name`, `role_id`) VALUES
(1, 'msantos', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Maria', 'Santos', 1),
(2, 'jreyes', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Jose', 'Reyes', 2),
(3, 'acruz', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Ana', 'Cruz', 2),
(4, 'rdelacruz', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Ramon', 'Dela Cruz', 3),
(5, 'lgarcia', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Luz', 'Garcia', 3),
(6, 'pbautista', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Paolo', 'Bautista', 2),
(7, 'cmendoza', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Carla', 'Mendoza', 4),
(8, 'ftorres', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Fernando', 'Torres', 4),
(9, 'hramos', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Hazel', 'Ramos', 1),
(10, 'dlopez', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Daniel', 'Lopez', 3);

--
-- Indexes for dumped tables
--

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
  ADD UNIQUE KEY `reference_no` (`reference_no`);

--
-- Indexes for table `tbl_stock_in_details`
--
ALTER TABLE `tbl_stock_in_details`
  ADD PRIMARY KEY (`stock_in_detail_id`),
  ADD KEY `fk_sid_stockin` (`stock_in_id`),
  ADD KEY `fk_sid_variant` (`variant_id`);

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
-- AUTO_INCREMENT for table `tbl_categories`
--
ALTER TABLE `tbl_categories`
  MODIFY `category_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `tbl_category_types`
--
ALTER TABLE `tbl_category_types`
  MODIFY `category_type_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `tbl_products`
--
ALTER TABLE `tbl_products`
  MODIFY `product_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `tbl_product_variants`
--
ALTER TABLE `tbl_product_variants`
  MODIFY `variant_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT for table `tbl_roles`
--
ALTER TABLE `tbl_roles`
  MODIFY `role_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `tbl_stock_ins`
--
ALTER TABLE `tbl_stock_ins`
  MODIFY `stock_in_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `tbl_stock_in_details`
--
ALTER TABLE `tbl_stock_in_details`
  MODIFY `stock_in_detail_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT for table `tbl_students`
--
ALTER TABLE `tbl_students`
  MODIFY `student_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `tbl_transactions`
--
ALTER TABLE `tbl_transactions`
  MODIFY `transaction_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `tbl_transaction_items`
--
ALTER TABLE `tbl_transaction_items`
  MODIFY `transaction_item_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- AUTO_INCREMENT for table `tbl_users`
--
ALTER TABLE `tbl_users`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `tbl_category_types`
--
ALTER TABLE `tbl_category_types`
  ADD CONSTRAINT `fk_type_category` FOREIGN KEY (`category_id`) REFERENCES `tbl_categories` (`category_id`);

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
-- Constraints for table `tbl_stock_in_details`
--
ALTER TABLE `tbl_stock_in_details`
  ADD CONSTRAINT `fk_sid_stockin` FOREIGN KEY (`stock_in_id`) REFERENCES `tbl_stock_ins` (`stock_in_id`),
  ADD CONSTRAINT `fk_sid_variant` FOREIGN KEY (`variant_id`) REFERENCES `tbl_product_variants` (`variant_id`);

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
