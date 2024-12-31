USE EduConnectDevelopmentDatabase

SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'GeneralExpertiseArea'

INSERT INTO Education.GeneralExpertiseArea (GeneralExpertiseAreaId, GeneralExpertiseAreaName, GeneralExpertiseAreaDescription, CreatedAt)
VALUES 
(NEWID(), 'Programming', 'Study of creating software applications through the use of coding languages and development tools.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'Data Science', 'Analysis and interpretation of large data sets to derive meaningful insights and support decision-making.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'Cybersecurity', 'Protecting digital systems and networks against unauthorized access, attacks, or damage.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'Artificial Intelligence', 'Development of systems that simulate human intelligence to perform tasks like decision-making and language processing.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'Graphic Design', 'Creation of visual content to communicate messages using typography, imagery, and layout techniques.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'Digital Marketing', 'Strategies to promote products or services using digital channels and online platforms.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'Web Development', 'Designing and building websites or web applications that are functional, responsive, and user-friendly.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'Business Management', 'Administration and organization of business activities to achieve defined objectives efficiently.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'Healthcare Technology', 'Application of technology to improve the delivery of healthcare services.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'Game Development', 'Creation of interactive entertainment through programming, storytelling, and design.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE()));
(NEWID(), 'Cloud Computing', 'The practice of using remote servers hosted on the internet to store, manage, and process data, instead of local servers or personal computers.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), 'Blockchain Technology', 'A decentralized digital ledger technology that records transactions across multiple computers in a secure, transparent, and immutable way.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE()))
SELECT DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())

SELECT *
FROM Education.GeneralExpertiseArea

INSERT INTO Education.SpecificExpertiseField (SpecificExpertiseFieldId, GeneralExpertiseAreaId, SpecificExpertiseFieldName, Description, CreatedAt)
VALUES 
(NEWID(), '420EF57F-E745-4D3F-89ED-552D45213BEE', 'Front-End Development', 'Crafting user interfaces with HTML, CSS, and JavaScript.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '420EF57F-E745-4D3F-89ED-552D45213BEE', 'Back-End Development', 'Server-side programming using databases and APIs.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '420EF57F-E745-4D3F-89ED-552D45213BEE', 'Mobile App Development', 'Building apps for Android and iOS devices.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '420EF57F-E745-4D3F-89ED-552D45213BEE', 'Game Programming', 'Developing game mechanics and logic.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '420EF57F-E745-4D3F-89ED-552D45213BEE', 'Embedded Systems Programming', 'Coding for hardware with limited resources.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '420EF57F-E745-4D3F-89ED-552D45213BEE', 'Database Management', 'Structuring and querying relational databases.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '420EF57F-E745-4D3F-89ED-552D45213BEE', 'DevOps', 'Integrating development and operations for software delivery.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '420EF57F-E745-4D3F-89ED-552D45213BEE', 'Functional Programming', 'Emphasizing function-based programming paradigms.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '420EF57F-E745-4D3F-89ED-552D45213BEE', 'Software Testing', 'Ensuring the quality of software through tests.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '420EF57F-E745-4D3F-89ED-552D45213BEE', 'Cloud Computing', 'Building scalable applications using cloud infrastructure.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE()))


--Data science -  '85DB41FE-9A44-4768-8180-DBDAF10B2C90',
INSERT INTO Education.SpecificExpertiseField (SpecificExpertiseFieldId, GeneralExpertiseAreaId, SpecificExpertiseFieldName, Description, CreatedAt)
VALUES 
(NEWID(), '85DB41FE-9A44-4768-8180-DBDAF10B2C90','Data Visualization', 'Creating visual representations of data.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '85DB41FE-9A44-4768-8180-DBDAF10B2C90','Machine Learning', 'Building predictive models from data.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '85DB41FE-9A44-4768-8180-DBDAF10B2C90','Statistical Analysis', 'Applying statistical techniques to data sets.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '85DB41FE-9A44-4768-8180-DBDAF10B2C90','Data Cleaning', 'Preprocessing data for accurate analysis.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '85DB41FE-9A44-4768-8180-DBDAF10B2C90','Big Data Analytics', 'Working with large, complex data sets.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '85DB41FE-9A44-4768-8180-DBDAF10B2C90','Business Intelligence', 'Extracting actionable insights from data.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '85DB41FE-9A44-4768-8180-DBDAF10B2C90','Text Mining', 'Analyzing text data for patterns.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '85DB41FE-9A44-4768-8180-DBDAF10B2C90','Predictive Analytics', 'Forecasting outcomes using data.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '85DB41FE-9A44-4768-8180-DBDAF10B2C90','Data Governance', 'Ensuring data quality and security.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '85DB41FE-9A44-4768-8180-DBDAF10B2C90','AI Integration', 'Using AI in data workflows.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE()))


-- Cybersecurity '5C2B40C2-06C7-41ED-9203-4ED32D28D592',
INSERT INTO Education.SpecificExpertiseField (SpecificExpertiseFieldId, GeneralExpertiseAreaId, SpecificExpertiseFieldName, Description, CreatedAt)
VALUES 
(NEWID(), '5C2B40C2-06C7-41ED-9203-4ED32D28D592', 'Ethical Hacking', 'Testing system vulnerabilities.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '5C2B40C2-06C7-41ED-9203-4ED32D28D592', 'Cryptography', 'Securing data through encryption.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '5C2B40C2-06C7-41ED-9203-4ED32D28D592', 'Network Security', 'Protecting network infrastructure.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '5C2B40C2-06C7-41ED-9203-4ED32D28D592', 'Cyber Threat Analysis', 'Identifying and mitigating cyber risks.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '5C2B40C2-06C7-41ED-9203-4ED32D28D592', 'Incident Response', 'Managing security breaches.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '5C2B40C2-06C7-41ED-9203-4ED32D28D592', 'Penetration Testing', 'Simulating attacks to identify weaknesses.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '5C2B40C2-06C7-41ED-9203-4ED32D28D592', 'Secure Coding', 'Developing secure software.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '5C2B40C2-06C7-41ED-9203-4ED32D28D592', 'Malware Analysis', 'Understanding malicious software behavior.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '5C2B40C2-06C7-41ED-9203-4ED32D28D592', 'Security Compliance', 'Adhering to data protection regulations.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '5C2B40C2-06C7-41ED-9203-4ED32D28D592', 'IoT Security', 'Safeguarding internet-connected devices.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE()))

-- Artificial Intelligence, '7208723F-C1C9-4438-9CE9-14D27C80B790',
INSERT INTO Education.SpecificExpertiseField (SpecificExpertiseFieldId, GeneralExpertiseAreaId, SpecificExpertiseFieldName, Description, CreatedAt)
VALUES 
(NEWID(),'7208723F-C1C9-4438-9CE9-14D27C80B790','Natural Language Processing', 'Teaching machines to understand human language.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'7208723F-C1C9-4438-9CE9-14D27C80B790','Computer Vision', 'Enabling machines to interpret visual data.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'7208723F-C1C9-4438-9CE9-14D27C80B790','Reinforcement Learning', 'Training models through rewards and penalties.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'7208723F-C1C9-4438-9CE9-14D27C80B790','AI Ethics', 'Exploring ethical implications of AI usage.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'7208723F-C1C9-4438-9CE9-14D27C80B790','Deep Learning', 'Using neural networks for complex tasks.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'7208723F-C1C9-4438-9CE9-14D27C80B790','Speech Recognition', 'Converting spoken language to text.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'7208723F-C1C9-4438-9CE9-14D27C80B790','AI in Robotics', 'Enhancing robots with AI capabilities.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'7208723F-C1C9-4438-9CE9-14D27C80B790','Recommender Systems', 'Creating personalized recommendations.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'7208723F-C1C9-4438-9CE9-14D27C80B790','AI in Healthcare', 'AI applications in medical diagnosis and treatment.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'7208723F-C1C9-4438-9CE9-14D27C80B790','Automated Decision Systems', 'AI systems for decision-making.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),

-- Graphic Design, '0A1A728A-6133-4E08-AD99-4B75FBF942CF',
INSERT INTO Education.SpecificExpertiseField (SpecificExpertiseFieldId, GeneralExpertiseAreaId, SpecificExpertiseFieldName, Description, CreatedAt)
VALUES 
(NEWID(), '0A1A728A-6133-4E08-AD99-4B75FBF942CF','Typography', 'Designing and arranging type in graphic projects.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '0A1A728A-6133-4E08-AD99-4B75FBF942CF','Logo Design', 'Creating distinctive visual symbols for brands.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '0A1A728A-6133-4E08-AD99-4B75FBF942CF','Packaging Design', 'Designing product packaging for visual impact.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '0A1A728A-6133-4E08-AD99-4B75FBF942CF','UX/UI Design', 'Designing user interfaces and improving user experience.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '0A1A728A-6133-4E08-AD99-4B75FBF942CF','Web Design', 'Creating visually appealing and functional websites.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '0A1A728A-6133-4E08-AD99-4B75FBF942CF','Advertising Design', 'Designing print and digital advertisements.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '0A1A728A-6133-4E08-AD99-4B75FBF942CF','Illustration', 'Drawing images for various media applications.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '0A1A728A-6133-4E08-AD99-4B75FBF942CF','Motion Graphics', 'Creating moving visuals for videos and digital content.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '0A1A728A-6133-4E08-AD99-4B75FBF942CF','Branding', 'Building a consistent visual identity for a company or product.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '0A1A728A-6133-4E08-AD99-4B75FBF942CF','Print Design', 'Designing for printed materials like brochures and flyers.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),


-- Digital Marketing, 'BF50E9E6-B06A-43AF-AF51-E4B40531ED77',
INSERT INTO Education.SpecificExpertiseField (SpecificExpertiseFieldId, GeneralExpertiseAreaId, SpecificExpertiseFieldName, Description, CreatedAt)
VALUES 
(NEWID(),'BF50E9E6-B06A-43AF-AF51-E4B40531ED77','Search Engine Optimization (SEO)', 'Improving website visibility in search engines.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'BF50E9E6-B06A-43AF-AF51-E4B40531ED77','Social Media Marketing', 'Promoting products through social media platforms.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'BF50E9E6-B06A-43AF-AF51-E4B40531ED77','Content Marketing', 'Creating and distributing valuable content for audiences.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'BF50E9E6-B06A-43AF-AF51-E4B40531ED77','Email Marketing', 'Sending targeted promotional messages via email.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'BF50E9E6-B06A-43AF-AF51-E4B40531ED77','PPC Advertising', 'Pay-per-click advertising on platforms like Google Ads.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'BF50E9E6-B06A-43AF-AF51-E4B40531ED77','Affiliate Marketing', 'Collaborating with partners to drive traffic and sales.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'BF50E9E6-B06A-43AF-AF51-E4B40531ED77','Influencer Marketing', 'Working with influencers to promote brands.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'BF50E9E6-B06A-43AF-AF51-E4B40531ED77','Online Reputation Management', 'Managing a brand’s online reputation and reviews.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'BF50E9E6-B06A-43AF-AF51-E4B40531ED77','Analytics and Reporting', 'Tracking and analyzing marketing campaign performance.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'BF50E9E6-B06A-43AF-AF51-E4B40531ED77','Conversion Rate Optimization', 'Improving website effectiveness for lead conversion.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE()))


-- Web Development, '15ED7BC9-F840-444A-AADE-3ED4AC4BF983',
INSERT INTO Education.SpecificExpertiseField (SpecificExpertiseFieldId, GeneralExpertiseAreaId, SpecificExpertiseFieldName, Description, CreatedAt)
VALUES 
(NEWID(), '15ED7BC9-F840-444A-AADE-3ED4AC4BF983','HTML/CSS', 'Fundamentals of creating and styling web pages.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '15ED7BC9-F840-444A-AADE-3ED4AC4BF983','JavaScript Development', 'Writing client-side scripts to make websites interactive.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '15ED7BC9-F840-444A-AADE-3ED4AC4BF983','API Development', 'Building and integrating web APIs for communication.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '15ED7BC9-F840-444A-AADE-3ED4AC4BF983','Web Hosting', 'Setting up and maintaining web servers for websites.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '15ED7BC9-F840-444A-AADE-3ED4AC4BF983','Full-Stack Development', 'Developing both the front-end and back-end of a web application.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '15ED7BC9-F840-444A-AADE-3ED4AC4BF983','E-commerce Development', 'Creating online stores and payment systems.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '15ED7BC9-F840-444A-AADE-3ED4AC4BF983','Progressive Web Apps (PWA)', 'Building apps that work like websites but with app-like features.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '15ED7BC9-F840-444A-AADE-3ED4AC4BF983','Responsive Design', 'Designing websites that work on all devices and screen sizes.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '15ED7BC9-F840-444A-AADE-3ED4AC4BF983','Web Security', 'Securing websites and applications from attacks.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '15ED7BC9-F840-444A-AADE-3ED4AC4BF983','Web Performance Optimization', 'Enhancing speed and performance of websites.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE()))


-- Business Management¸, '86726AA9-8BCA-4118-A631-BAB6E50FD061'.
INSERT INTO Education.SpecificExpertiseField (SpecificExpertiseFieldId, GeneralExpertiseAreaId, SpecificExpertiseFieldName, Description, CreatedAt)
VALUES 
(NEWID(),'86726AA9-8BCA-4118-A631-BAB6E50FD061', 'Project Management', 'Planning, organizing, and overseeing projects.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'86726AA9-8BCA-4118-A631-BAB6E50FD061', 'Operations Management', 'Managing business operations for efficiency.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'86726AA9-8BCA-4118-A631-BAB6E50FD061', 'Human Resources Management', 'Overseeing employee recruitment and relations.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'86726AA9-8BCA-4118-A631-BAB6E50FD061', 'Financial Management', 'Managing financial resources and investments.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'86726AA9-8BCA-4118-A631-BAB6E50FD061', 'Marketing Management', 'Leading marketing efforts and campaigns.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'86726AA9-8BCA-4118-A631-BAB6E50FD061', 'Sales Management', 'Managing and leading sales teams and processes.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'86726AA9-8BCA-4118-A631-BAB6E50FD061', 'Risk Management', 'Identifying and mitigating business risks.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'86726AA9-8BCA-4118-A631-BAB6E50FD061', 'Strategic Management', 'Setting and achieving long-term business goals.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'86726AA9-8BCA-4118-A631-BAB6E50FD061', 'Leadership Skills', 'Developing effective leadership and management techniques.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(),'86726AA9-8BCA-4118-A631-BAB6E50FD061', 'Business Analytics', 'Analyzing business data to improve performance.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE()))


-- Healthcare Technology, '738203D7-F15F-4A9B-8B5A-B5E6EE151128',
INSERT INTO Education.SpecificExpertiseField (SpecificExpertiseFieldId, GeneralExpertiseAreaId, SpecificExpertiseFieldName, Description, CreatedAt)
VALUES 
(NEWID(), '738203D7-F15F-4A9B-8B5A-B5E6EE151128', 'Health Information Systems', 'Managing and utilizing healthcare data systems.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '738203D7-F15F-4A9B-8B5A-B5E6EE151128', 'Telemedicine', 'Providing healthcare services through telecommunication technologies.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '738203D7-F15F-4A9B-8B5A-B5E6EE151128', 'Medical Device Development', 'Designing and building medical technologies.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '738203D7-F15F-4A9B-8B5A-B5E6EE151128', 'Health Data Analytics', 'Using data to improve healthcare outcomes.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '738203D7-F15F-4A9B-8B5A-B5E6EE151128', 'Health IT Security', 'Protecting healthcare data and systems from cyber threats.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '738203D7-F15F-4A9B-8B5A-B5E6EE151128', 'Clinical Research', 'Conducting studies to improve healthcare practices.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '738203D7-F15F-4A9B-8B5A-B5E6EE151128', 'Electronic Health Records (EHR)', 'Managing patient records electronically.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '738203D7-F15F-4A9B-8B5A-B5E6EE151128', 'AI in Healthcare', 'Applying artificial intelligence to medical fields.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '738203D7-F15F-4A9B-8B5A-B5E6EE151128', 'Healthcare Compliance', 'Ensuring compliance with healthcare regulations and laws.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '738203D7-F15F-4A9B-8B5A-B5E6EE151128', 'Patient Monitoring Systems', 'Designing systems for continuous patient health monitoring.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE()))


-- Game Development, '07F9B2E2-F897-4346-9389-858F5AB3A6C0',
INSERT INTO Education.SpecificExpertiseField (SpecificExpertiseFieldId, GeneralExpertiseAreaId, SpecificExpertiseFieldName, Description, CreatedAt)
VALUES 
(NEWID(), '07F9B2E2-F897-4346-9389-858F5AB3A6C0', 'Game Design', 'Creating the concept, rules, and mechanics of a game.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '07F9B2E2-F897-4346-9389-858F5AB3A6C0', '3D Modeling and Animation', 'Creating 3D models and animations for games.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '07F9B2E2-F897-4346-9389-858F5AB3A6C0', 'Game Programming', 'Writing code for game mechanics and behavior.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '07F9B2E2-F897-4346-9389-858F5AB3A6C0', 'Game Testing', 'Ensuring quality through game testing and bug fixing.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '07F9B2E2-F897-4346-9389-858F5AB3A6C0', 'Artificial Intelligence for Games', 'Developing AI for non-player characters and behaviors.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '07F9B2E2-F897-4346-9389-858F5AB3A6C0', 'Virtual Reality (VR) Development', 'Building games and experiences for VR platforms.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '07F9B2E2-F897-4346-9389-858F5AB3A6C0', 'Game Sound Design', 'Creating sound effects and music for games.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '07F9B2E2-F897-4346-9389-858F5AB3A6C0', 'Game Networking', 'Designing multiplayer online game networks.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '07F9B2E2-F897-4346-9389-858F5AB3A6C0', 'Game Monetization', 'Strategies to make games profitable through ads and purchases.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '07F9B2E2-F897-4346-9389-858F5AB3A6C0', 'Mobile Game Development', 'Developing games for smartphones and tablets.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE()))


-- Cloud Computing, '3C6A0944-AEDE-444E-BFE9-FCDAF36268E90,
INSERT INTO Education.SpecificExpertiseField (SpecificExpertiseFieldId, GeneralExpertiseAreaId, SpecificExpertiseFieldName, Description, CreatedAt)
VALUES 
(NEWID(), '3C6A0944-AEDE-444E-BFE9-FCDAF36268E90' ,'Cloud Architecture', 'Designing and managing cloud infrastructure to ensure scalability, security, and reliability.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '3C6A0944-AEDE-444E-BFE9-FCDAF36268E90' ,'Cloud Security', 'Implementing security measures to protect data and applications in the cloud.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '3C6A0944-AEDE-444E-BFE9-FCDAF36268E90' ,'Cloud Deployment', 'Deploying applications and services in cloud environments.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '3C6A0944-AEDE-444E-BFE9-FCDAF36268E90' ,'Cloud Service Models', 'Understanding the different cloud service models such as IaaS, PaaS, and SaaS.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '3C6A0944-AEDE-444E-BFE9-FCDAF36268E90' ,'Serverless Computing', 'Building and deploying applications without managing the underlying servers.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '3C6A0944-AEDE-444E-BFE9-FCDAF36268E90' ,'Containerization', 'Using containers (like Docker) to deploy and manage applications in the cloud.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '3C6A0944-AEDE-444E-BFE9-FCDAF36268E90' ,'Cloud Automation', 'Automating tasks and processes in the cloud to improve efficiency and scalability.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '3C6A0944-AEDE-444E-BFE9-FCDAF36268E90' ,'Hybrid Cloud', 'Integrating both private and public cloud resources for flexible cloud solutions.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '3C6A0944-AEDE-444E-BFE9-FCDAF36268E90' ,'Cloud Data Storage', 'Storing and managing data in the cloud in a secure and scalable manner.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '3C6A0944-AEDE-444E-BFE9-FCDAF36268E90' ,'Cloud Computing Platforms', 'Working with cloud platforms like AWS, Azure, and Google Cloud to build solutions.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE()))

--Blockchain Technology, '01689D67-3B70-4E9E-A9A0-736F870C63B4',
INSERT INTO Education.SpecificExpertiseField (SpecificExpertiseFieldId, GeneralExpertiseAreaId, SpecificExpertiseFieldName, Description, CreatedAt)
VALUES 
(NEWID(), '01689D67-3B70-4E9E-A9A0-736F870C63B4', 'Blockchain Development', 'Designing and building decentralized applications (dApps) using blockchain technology.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '01689D67-3B70-4E9E-A9A0-736F870C63B4', 'Cryptocurrency', 'Understanding and working with digital currencies like Bitcoin and Ethereum that use blockchain technology.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '01689D67-3B70-4E9E-A9A0-736F870C63B4', 'Smart Contracts', 'Creating and deploying self-executing contracts with the terms of the agreement written directly into code.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '01689D67-3B70-4E9E-A9A0-736F870C63B4', 'Blockchain Security', 'Implementing measures to secure blockchain networks and protect data from unauthorized access.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '01689D67-3B70-4E9E-A9A0-736F870C63B4', 'Decentralized Finance (DeFi)', 'Developing and using financial applications and services built on blockchain platforms.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '01689D67-3B70-4E9E-A9A0-736F870C63B4', 'Blockchain for Supply Chain', 'Using blockchain to enhance transparency and traceability in supply chain management.', DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '01689D67-3B70-4E9E-A9A0-736F870C63B4', 'Blockchain Integration', 'Integrating blockchain technology with existing systems and processes.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '01689D67-3B70-4E9E-A9A0-736F870C63B4', 'Blockchain Governance', 'Managing and regulating blockchain networks and ensuring compliance with laws and standards.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '01689D67-3B70-4E9E-A9A0-736F870C63B4', 'NFT Development', 'Creating and managing Non-Fungible Tokens (NFTs) for digital assets and collectibles.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE())),
(NEWID(), '01689D67-3B70-4E9E-A9A0-736F870C63B4', 'Distributed Ledger Technology', 'Implementing and working with distributed ledgers to improve transparency and reliability of data.',DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE()))
DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00', GETUTCDATE()))
SELECT *
FROM Education.SpecificExpertiseField
SELECT *
FROM Education.GeneralExpertiseArea