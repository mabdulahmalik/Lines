insert [usr].[UserStatus] ([UserId], [Availability], [ChangedAt], [Message])
select '{30400edb-a083-4efc-a823-86b6599e8811}', 1, '2024-10-24 14:18:38.4272196', NULL UNION ALL
select '{896b1095-a603-4f3d-8ed4-76b46d1be0a6}', 0, '2024-10-24 14:18:38.4272196', NULL UNION ALL
select '{fa455caf-fd17-4c97-be45-64b856783e88}', 2, '2024-10-24 14:18:38.4272196', N'dealing with an unruley patient.'

insert [usr].[UserProfileExt] ([UserId], [Preferences], [InTraining])
select '{30400edb-a083-4efc-a823-86b6599e8811}', 7, 0 UNION ALL
select '{896b1095-a603-4f3d-8ed4-76b46d1be0a6}', 7, 1 UNION ALL
select '{fa455caf-fd17-4c97-be45-64b856783e88}', 7, 0