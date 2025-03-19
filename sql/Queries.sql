INSERT INTO public."Users" ("Username", "PassHash", "CreatedOn")
VALUES
('Alice', 'hash123', NOW()),
('Bob',   'hash456', NOW());

INSERT INTO "Categ" ("CatName", "CatInfo", "UserId", "CreatedOn") VALUES
('Groceries', 'Food and household items', 2, NOW()),
('Transport', 'Fuel, tickets, public transport', 2, NOW()),
('Entertainment', 'Movies, concerts, subscriptions', 2, NOW()),
('Healthcare', 'Doctor visits, medicine', 2, NOW()),
('Salary & Income', 'All income sources', 2, NOW());


INSERT INTO "PayWays" ("PayLabel", "PayInfo", "UserId", "CreatedOn") VALUES
('Credit Card', 'Visa/Mastercard payments', 2, NOW()),
('Cash', 'Physical money payments', 2, NOW()),
('Bank Transfer', 'Direct payments from bank account', 2, NOW());

INSERT INTO "PlanBuds" ("CatId", "LimitAmt", "PeriodTxt", "UserId", "CreatedOn") VALUES
(1, 500.00, '2025-01', 2, NOW()),
(2, 150.00, '2025-01', 2, NOW()),
(3, 200.00, '2025-02', 2, NOW()),
(4, 300.00, '2025-02', 2, NOW()),
(5, 1000.00, '2025-03', 2, NOW()),
(1, 600.00, '2025-03', 2, NOW()),
(2, 200.00, '2025-04', 2, NOW()),
(3, 250.00, '2025-04', 2, NOW()),
(4, 350.00, '2025-05', 2, NOW()),
(5, 1200.00, '2025-05', 2, NOW());

INSERT INTO "CashFlows" ("FlowDesc", "FlowAmount", "FlowDate", "FlowType", "CatId", "PayId", "UserId", "CreatedOn") VALUES
--  (expenses)
('Groceries at supermarket', -75.50, '2025-03-10', 'expense', 1, 2, 2, NOW()),
('Fuel for car', -40.00, '2025-03-12', 'expense', 2, 1, 2, NOW()),
('Netflix subscription', -15.99, '2025-03-15', 'expense', 3, 3, 2, NOW()),
('Doctor visit', -100.00, '2025-03-18', 'expense', 4, 1, 2, NOW()),
('Dinner at restaurant', -50.00, '2025-03-20', 'expense', 1, 2, 2, NOW()),

-- (income)
('Salary payment', 2500.00, '2025-03-05', 'income', 5, 3, 2, NOW()),
('Freelance project', 800.00, '2025-03-10', 'income', 5, 3, 2, NOW()),
('Stock dividends', 120.00, '2025-03-15', 'income', 5, 3, 2, NOW()),
('Sold old bike', 150.00, '2025-03-18', 'income', 5, 2, 2, NOW()),
('Bonus from work', 500.00, '2025-03-25', 'income', 5, 3, 2, NOW()),

-- another
('Concert ticket', -60.00, '2025-04-01', 'expense', 3, 1, 2, NOW()),
('Bus pass', -30.00, '2025-04-05', 'expense', 2, 2, 2, NOW()),
('Gym membership', -45.00, '2025-04-10', 'expense', 4, 1, 2, NOW()),
('New phone case', -20.00, '2025-04-12', 'expense', 1, 3, 2, NOW()),
('Flight tickets', -300.00, '2025-04-20', 'expense', 2, 1, 2, NOW());

