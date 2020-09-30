# Rate Your Government Website Application
Website Application that allows for **documenting** and rating _unlawful_ or _heroic_ encounters with Government Institutions and Civil Servants in order to create a publically accesible, searchable register.

### Application Use Case
The guiding principle behind the application is that government, it's institutions and employees _(in their professional capacity)_ should be accountable to the citizens of their country.

The reality is often that general public opinion is influenced by increasingly biased or inaccurate news sources and is limited to incidents egregious enough to reach the news or social media.  There is a lack of access to quantitative information, especially at local government levels.  

For example, an individual may have a particularly bad experience with a civil servant or governmental institution but feel like they have no recourse as they simply do not have the information available to know if the behavior is typical or legal for the specific civil servant or institution, so they might feel powerless to do anything to address the situation and simply accept it.     

This could lead to the civil servant believing that their behavior is appropriate, to perpetuation of the behavior and, worst case scenario, for this attitude to spread and become endemic in the institution.

Similarly, there is a lack of quantitative information on government institutions and civil servants doing a **great** job of serving the public.  

### The Goal of this application
This application is intended to provide the tools to create a database of citizen’s government interactions where their rights were violated or they were discriminated against, or they felt that a civil servant stood out from the crowd by going above and beyond to assist them (with a great understanding of their legal and humanitarian rights) in order to create a publicly available register of the performance of government institutions and the civil servants that they employ using taxes paid by citizens.

_Having access to quantitative and specific information would, at the very least, make it more readily possible for:_ 
- Individuals to connect with others sharing similar experiences as a support system or to offer advice on what they could do to get justice. 
- The identification of “repeat offenders”, which could lead to civilians mobilizing to redress the situation through complaints, petitions or appeals to the appropriate authorities.
- Citizens to make better informed voting decisions if the civil servant’s position is elected.  
- Identification of a potential need for new legislation to be introduced to address the behavior.  
- Purging government of civil servants whose actions regularly violate the rights of citizens and make it more difficult for them to simply move to a region where government institutions employing them may not be aware of their past behavior.   
- Promotion of a culture of mutual respect by encouraging the recognition and promotion or election of good civil servants who respect the rights of citizens.   

### What this application is NOT intended for 
- Harassment of civil servants in their personal capacity.
- Malicious retribution, [Doxing involving Civil Servant's Private Information](https://en.wikipedia.org/wiki/Doxing) or [Slander](https://en.wikipedia.org/wiki/Defamation).
- Discrimination against **anyone** based on race, gender, religious or other beliefs, disabilities, sexual orientation or identity.  
- Depriving _any individual_ of their legal or human rights.  

In order to discourage this, the application is designed to focus on **behavior** (interactions - incidents), which also encourages objectivity by allowing the consumer of the information to form their own opinions based on facts presented.  

In a nutshell, in the words of _Mahatma Gandhi_, **"Be the change you wish to see in the world"**.

### Application Functionality
**_Note that the application is currently in the Development Phase_**

_Each phase implicitly includes a functional improvements/refactoring, testing and bug fix feedback loop which must be completed before moving to the next phase._

*Functionality to be introduced in Phase 1*
- Target running on a Windows host with a MSSQL 2019 Database.
- Server level (i.e. for the entire website, not at an individual user level) implementation of language and culture based on configuration settings.
- Implement only language resource files and culture in English for the USA in this version of the application. 
- Create User Terms and Conditions, Privacy and Cookie Policy Templates
- User Registration and secure login using User Names for the primary login using Microsoft Blazor OAuth EF technology included with the Blazor Web Server project template.
- Registration Confirmation using unique email address which will also be required but not used to log in.
- Create the functionality for users to change their email address.
- Allow only users from the same country as the Server Application Configuration Settings to register and log in. 
- Allow only logged in users from the same country as the Server Configuration Settings to to create records for incidents, civil servants and government institutions. 
- Allow anonymous view only access to Website including Institution/Civil Servant/Incident search capabilities.

*Provisional Functionality to be introduced in Phase 2*
- Create Modular comments section which will eventually be able to be attached to civil servants, government institutions and incidents (and eventually other data entities in future versions), but implement only against incidents in this phase as MVP proof of concept in order to gather feedback and implement improvements.

*Provisional Functionality to be introduced in Phase 3*
- Implement Moderator and Administrator roles/menus.
- Add functionality for an Administrator to block user accounts
- Implement Moderation functionality in the comments section.

*Provisional Functionality to be introduced in Phase 4*
- Allow users from other countries to register, log in and comment only.

*Provisional Functionality to be introduced in Phase 5*
- Create a "Resources" section and allow Administrators/Moderators/Users to update it with useful links to sources for help or information useful to the targeted audience, like links to statutes, organizations offering support, complaint resources etc.

*Provisional Functionality to be introduced in Phase 6*
- Add comments to government institutions

*Provisional Functionality to be introduced in Phase 7*
- Add comments to civil servants

*Provisional Functionality to be introduced in Phase 8*
- Add summary metrics and reporting for all entities

*Provisional Functionality to be introduced in Phase 9*
_GDPR:_
- Add ability for users to download their private data
- Add functionality for a user to delete their account
- Add functionality for an Administrator to delete user accounts

_**Publish of v1**_
- Add Installation instructions and publish Official Release v1


_**Potential Functionality to be Considered for future releases (in no particular order):**_
- Add system maintenance service application and API functionality as well as [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/) to perform system maintenance, check for spam comments, send notification emails, etc.
- Add functionality to allow for individual or digest email notification to users on comment replies or mentions dependent on user preferences 
- Add calendar and allow users based in the same country as the server to schedule events
- Allow for creation and publication of Blog Posts/Newsletters/Articles
- Allow users to input (https://www.change.org/), (https://secure.avaaz.org/page/en/), etc. petitions to the site and determine strategies to raise awareness among registered users (Likely using Blog Posts and/or Incident Links)
- Allow for the creation of official registers, like the [Brady List](https://en.wikipedia.org/wiki/Brady_disclosure) in the US and compilation of information from those registers in a single, searchable location.  
- Allow for creation and publication of FAQ section 
- Allow for creation and distribution of Alerts, (e.g. of protesters being arrested; traffic police pulling people over to extract bribes, etc.)  
- Allow for users to subscribe to regions/cities/counties/states/provinces and send emails making them aware of new incidents/calendar events/alerts/blog posts/articles/official register entries in those places
- Add hashtag functionality 
- Add functionality to share things to Facebook, Twitter, etc.
- Add functionality to allow Google ads to be switched on - generic or user specific dependent on user preferences 
- Add functionality for a user to choose an avatar as their profile picture
- Add functionality for a user to upload an image of their choice to use as their profile picture
- Create MySQL Database Library 
- Create MariaDB Database Library 
- Create Linux Port Web Host Publish package and instructions
- Allow for optional configuration to use and implement Google ads to contribute to hosting costs incurred
- Allow for optional acceptance of PayPal donations


### Core Design Principles
- **User Data Protection is key.**  Collect only the minimum essential personal information required to provide the necessary functionality and go above and beyond to protect personal information. 
- Respect [EU GDPR Guidelines](https://en.wikipedia.org/wiki/General_Data_Protection_Regulation) as much as is practically possible. 
- Use an "double opt in" approach for all email communications and potential targeted advertising.
- Design the Website Application to allow for disability accessibility by respecting [WAI-ARIA](https://en.wikipedia.org/wiki/WAI-ARIA) [Authoring Practices](https://www.w3.org/TR/wai-aria-practices/). 
- Design for Progressive Enhancement but consider the future path and adopt a balanced approach.
- Consider affordability of hosting in the fundamental design, including but not limited to consideration of maximum DB size, bandwidth caps, etc.
- Design with _sensible_ [Separation of Concerns](https://en.wikipedia.org/wiki/Separation_of_concerns) in mind to allow for maximum future flexibility and MVP. 

### Development Approach
- MVP [Minimum Viable Product](https://en.wikipedia.org/wiki/Minimum_viable_product) approach to allow for a continuous feedback loop that can be implemented with limited refactoring of code.
- _Keep it Simple._  Consider debugging and maintainability of code by others when coding. 
- Pay attention to compatibly of licensing when including third party components. 
- Keep your confidential development machine application settings and secrets on **your local machine only** by using _appsettings.Development.json_ or VS Secrets Store, etc. 


### Last Updated
30 September 2020 UTC
