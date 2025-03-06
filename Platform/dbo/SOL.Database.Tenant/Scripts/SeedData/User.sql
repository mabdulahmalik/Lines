insert [usr].[UserStatus] ([UserId], [Availability], [ChangedAt], [Message])
select '{6a7c7d5a-e560-4172-adcf-4028c0807c2e}', 1, '2024-10-24 14:18:38.4272196', NULL UNION ALL
select '{a024b775-13b8-4d2a-9170-dc1ea01c4d60}', 0, '2024-10-24 14:18:38.4272196', NULL UNION ALL
select '{0ccdf7be-c987-4f3f-b7fe-5c661028e500}', 2, '2024-10-24 14:18:38.4272196', N'dealing with an unruley patient.' UNION ALL
select '{7bddef79-1709-4db2-acd4-b47c8c8f4403}', 0, '2024-10-24 14:18:38.4272196', NULL

insert [usr].[UserProfileExt] ([UserId], [Preferences], [InTraining])
select '{6a7c7d5a-e560-4172-adcf-4028c0807c2e}', 7, 0 UNION ALL
select '{a024b775-13b8-4d2a-9170-dc1ea01c4d60}', 7, 1 UNION ALL
select '{0ccdf7be-c987-4f3f-b7fe-5c661028e500}', 7, 1 UNION ALL
select '{7bddef79-1709-4db2-acd4-b47c8c8f4403}', 7, 0