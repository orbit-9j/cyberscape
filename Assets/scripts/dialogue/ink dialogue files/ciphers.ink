===main===
Where are we?  #speaker:You #portrait:you_confused 

Looks like a warehouse of sorts. This must be where they receive supplies for the lab. #speaker:Console #portrait:console_neutral

Where's the lab? #speaker:You #portrait:you_confused 

It must be on a different floor. #speaker:Console #portrait:console_neutral

But there isn't a button for higher floors here, only down... #speaker:You #portrait:you_confused 

Then the elevator must be controlled through some sort of signal rather than a button. Let's look around. #speaker:Console #portrait:console_neutral
->DONE


=== ciphers ===
Dammit, I can't understand what they're saying! Long Dusk's communications are all encrypted! #speaker:Console #portrait:console_wat

What does that mean? #speaker:You #portrait:you_confused

\*sigh\* Looks like it's time to teach you about ciphers. #speaker:Console  #portrait:console_neutral
Ciphers encode text in a way that makes it impossible for someone to read it without the secret key.
This makes comunication secure, so you can share your secrets without anyone learning them.

Does that mean we will never be able to get to the other floors? I guess that's mission failed, we'll get 'em next time... #speaker:You  #portrait:you_sad

Not so fast! It looks like these robots use a substitution cipher called Caesar cipher for their messages. #speaker:Console  #portrait:console_evilexcited

Is that good? #speaker:You  #portrait:you_neutral

Yeah, you're in luck - this cipher is quite easy to break! #speaker:Console  #portrait:console_evilexcited
Let me explain some cipher terminology to you first - you have to know your enemy before you can defeat it. #portrait:console_neutral

Quick, give me a sentence! #portrait:console_evilexcited

Uh..! What kind of sentenc- #speaker:You  #portrait:you_alarmed

Perfect! Now, the message "What kind of sentence?" is currently not encoded, and we call that PLAINTEXT. #speaker:Console  #portrait:console_evilexcited

W-wait! I didn't mea- #speaker:You  #portrait:you_alarmed

#puzzle:caesarCipherDemo

Oh! #speaker:You  #portrait:you_alarmed

This screen should help you understand the cipher. #speaker:Console  #portrait:console_neutral
Feel free to play around with it using your arrow keys.

Now we can pick a random number between 1 and 26. This will be our ENCRYPTION KEY. Let's say the key is 4. #speaker:Console  #portrait:console_evilexcited

Why is it between 1 and 26? Can we not change that range? #speaker:You  #portrait:you_confused

That's because the Caesar cipher works by shifting the plaintext alphabet by a certain number of letters.  #speaker:Console  #portrait:console_neutral
Since there are 26 letters in the english alphabet, a key of more than 26 will wrap around back to the start of the alphabet. 
This will be equivalent to starting counting from 0 again, so it makes no sense to have a key bigger than 26.
Also, a key of 0 is the same as not encrypting the plaintext, since the plaintext and ciphertext 
alphabets will be the same, in other words there will be no offset. 
Therefore the key must be above 0.

So how do we use this key then? #speaker:You #portrait:you_confused

The encryption key will encode the plaintext into gibberish that no one can understand. #speaker:Console  #portrait:console_neutral
The encoding process is called ENCRYPTION and the resulting encoded text is called CIPHERTEXT.
Since our key is 4, to encrypt the plaintext we need to replace each plaintext letter with the letter 4 places to the right from it in the 
alphabet.
For example, the letter A will be relaced with E, because E is 4 places down the alphabet.

But the first letter of our plaintext is W!  W - X - Y - Z, that's only 3 hops and the alphabet has ended. How will that  #speaker:You  #portrait:you_confused
work?

This is where the wrapping comes into play! We treat the alphabet like a loop rather than a finite line, so once you reach the end  #speaker:Console  #portrait:console_neutral
you hop right back to the start.

Does that mean that W will become A because A is at the start of the alphabet? #speaker:You  #portrait:you_confused

Yes, exactly! Because we only needed to hop one more place after Z, the letter W will map to the letter A. If you needed to encode 
the letter Y with the key of 4, which letter will it become? #speaker:Console  #portrait:console_neutral
...
* [A] 
    -> ciphers.wrong("A")
* [B] 
    -> ciphers.wrong("B")
* [C]
    -> ciphers.correct("C")

= wrong(answer)
{answer}? That's not quite right #portrait:console_sad
-> continue

= correct(answer)
...
{answer}? Yes, that's correct! #portrait:console_evilexcited
-> continue

= continue

Because we need to do 4 hops from Y, Y - Z - A - B - C will land on C, so the letter Y will be encoded to become the letter C. #portrait:console_neutral
Now that you understand the Caesar cipher, let's encrypt our plaintext "What kind of sentence?". #portrait:console_neutral
The ciphertext is "Alex omrh sj wirxirgi". #portrait:console_evilexcited

Huh? It has the name Alex in it, what does that mean? #speaker:You  #portrait:you_confused

Haha, it doesn't actually mean anything. It just so happened that encrypting the word "What" with a key of 4 transformed it into #speaker:Console  #portrait:console_evilexcited
what looks like a real word. 
#puzzle:closePuzzle
You will likely find lots of other plaintexts and keys that result in curious ciphertexts on your encryption journey. #portrait:console_neutral

You say that as if I will need to know about encryption outside of this quest. #speaker:You  #portrait:you_confused

Actually, yes you will! When you interact with computers, encryption is all around you. #speaker:Console  #portrait:console_neutral

What, where? #speaker:You  #portrait:you_confused

Most computer communications these days are encrypted. #speaker:Console  #portrait:console_neutral
For example, when you access the internet your search queries are encrypted so that no one can listen in on what weird
stuff you're searching for.
Not just your search queries, but also any personal information you enter on websites through forms.
Your username and password, your credit card details, your address etc. 
Encryption keeps you safe from identity theft and hacking.

Wait, you said "most communications" and not "all". How can I know if my internet activity is being encrypted? #speaker:You  #portrait:you_confused

There's one easy tip for that: look out for links that start with "https". #speaker:Console  #portrait:console_neutral
Those are encrypted, and the "s" in "https" stands for "secure". 
Some links start with "http". Those websites do not encrypt your activity, so be careful about what you type on there!
You can also look for a closed padlock icon somewhere on your URL bar. 
Conversely, an open padlock icon means the website isn't using encryption.

Ah, so now I know why my browser gave me an open padlock warning when I wanted to download free Cavebuild... #speaker:You  #portrait:you_neutral

Then why did you ignore it? Warnings are meant to be followed, it's a measure to keep you safe! #speaker:Console  #portrait:console_wat

I wanted to get Cavebuild for free! #speaker:You  #portrait:you_neutral

People like you are the reason humans aren't a Type II civilisation yet! #speaker:Console  #portrait:console_evilexcited
Nevermind. #portrait:you_neutral
Let's go intercept a message from one of the robots and I will teach you how to break this cipher!  #portrait:console_evilexcited
We need to find the cipher key to access the elevator again. #portrait:console_neutral
-> DONE

VAR key = 0
VAR ciphertext = ""
=== decrypt ===
//the key is {key}
//the ciphertext is {ciphertext}//debug

Looks like this robot is talking to the door. #speaker:Console  #portrait:console_neutral
Let's see if we can send an encrypted message to the door pretending to be the robot asking to open it.

Don't we need to know the encryption key that the robot and door share if we want to pretend to be the robot? #speaker:You  #portrait:you_confused

That's correct, we will first have to figure out the key. #speaker:Console  #portrait:console_neutral
Let's listen in on what the robot is saying! #portrait:console_evilexcited

{ciphertext} #speaker:Robot  #portrait:robot_smug

That sounds like a different language! How will we manage to find the right key to decipher this? #speaker:You  #portrait:you_confused

We could perform a brute force attack! #speaker:Console  #portrait:console_evilexcited

That sounds violent! #speaker:You  #portrait:you_alarmed

Haha, no, there will not be any physical violence involved. #speaker:Console  #portrait:console_neutral
A BRUTE FORCE ATTACK uses trial and error to find the correct answer, in this case the encryption key.
We will try to use every possible key to find the correct key for this cipher, and decrypt the message. 
If the resulting plaintext looks like a normal sentence, then we know we found the key.

I think I understand that, but what exactly does decryption mean? #speaker:You  #portrait:you_confused

DECRYPTION is simply converting ciphertext to plaintext. It's just another word for decoding an encoded message. #speaker:Console  #portrait:console_neutral
Together, an encryption algorithm and a decryption algorithm make up a CIPHER. 
They both share one key.
For a brute force attack, we don't need to know the Caesar Cipher decryption algorithm. #portrait:console_evilexcited

But if we did, how would we do it?  #speaker:You  #portrait:you_confused

We just have to shift the alphabet the key number of places, but in the opposite direction! #speaker:Console  #portrait:console_neutral

Alright, so how do we perform the brute force attack? #speaker:You  #portrait:you_confused

First, you will need to remember how many possible keys an English Caesar cipher has. #speaker:Console  #portrait:console_neutral

...
* [12]
    -> decrypt.wrong("12")
* [26]
    -> decrypt.correct("26")
* [1 million]
    -> decrypt.wrong("1 million")
    //don't actually have to do the dot notation, it just knows

= wrong(answer)
...
{answer}? That doesn't sound right... #portrait:console_sad
-> continue

=correct(answer)
...
{answer}? Yes, that's correct!  #portrait:console_evilexcited
-> continue

=continue
Because the Caesar cipher performs alphabetic shifts, it has as many possible keys as letters in the English alphabet.  #portrait:console_neutral
Remember, you can't shift a letter by more steps than the number of letters in the alphabet! Otherwise it will just wrap back to 0
and start again!
#puzzle:bruteForcePuzzle

Oh! What's this? #speaker:You  #portrait:you_confused

It's the Bruce wheel! It's short for brute force wheel. I just made that name up. #speaker:Console  #portrait:console_evilexcited

So what does it do? #speaker:You  #portrait:you_confused

You can use the left and right arrow keys to rotate the wheel. #speaker:Console  #portrait:console_neutral
Each sector on the wheel represents a Caesar cipher key.
You can perform a brute force attack by trying out each key on the wheel until the message looks like real words.
That will mean you found the right key. Press enter to check your answers.
Alright, I'll give it a try. #speaker:You  #portrait:you_happy
->DONE


=== bruteForceMinigame ====
//#puzzle:closePuzzle
Good job on your very first brute force attack! I'll make you into a real cyber criminal in no time! #speaker:Console  #portrait:console_evilexcited

W-what?! #speaker:You  #portrait:you_alarmed
I just want to get this over with so I can play Cavebuild! #portrait:you_neutral

Anyway. From the brute force attack we now know that the message was encrypted with the key of {key}. #speaker:Console  #portrait:console_neutral
This means that every letter in the original message was shifted to the letter {key} places away from it in the alphabet.
We can now try to encrypt our own message and send it to the door to open it.

-> DONE

//ideas for the future:
//have a sequence where we have to decrypt a message to showcase reversible keys
//explain why brute force is not always efficent - this ciper only has 26 keys but real ciphers can have millions and billions and trillions which takes much much longer to crack
//frequency analys attack
//explain substitution cipher vs caesar cipher - final boss-type puzzle
//why ciphers are needed in irl - encryption of internet traffic (online messaging, online banking, transactions)

=== encrypt ===

-> DONE

=== battle ===
Wait, you are not authorised to be here!  #speaker:Guard Bot  #portrait:Default
Our state-of-the-art security system will ensure you will never get through this door!

Uh-oh, guess it's time to put what we learned to the test! #speaker:Console  #portrait:console_evilexcited
#puzzle:battle
-> DONE

===battleStart===
Wait, I don't know how to fight this robot! #speaker:You  #portrait:you_alarmed

Well, here's the battle plan: we're gonna send the robot terminal commands that have a high chance of shutting it down,#speaker:Console  #portrait:console_neutral
 hoping one of them will work.
Because the robot only communicates with encrypted messages, you'll have to encrypt your commands using the caesar 
cipher I told you about earlier.
The guard bot will try to change its ecryption key often so you will have to perform a brute force attack and intecept the message
containing the new key.
Remember that key and use it for your next brute force attack!

Okay, I can do that. But how do I encrypt and decrypt? #speaker:You  #portrait:you_confused

Use the arrow keys to shift the plaintext alphabet when decrypting, and the ciphertext alphabet when encrypting. #speaker:Console  #portrait:console_neutral
After a few rounds of the brute force attack, you're gonna find a command that deals a lot of damage.
That's your "attack" command.
Sit back and enjoy those few seconds of accomplishment, but keep in mind that you will have to keep trying commands again until 
the robot shuts down!

What if I need to defend myself? #speaker:You #portrait:you_confused

If you weaken the robot enough, it won't do a strong attack. #speaker:Console  #portrait:console_neutral
It will only hurt you when you fail to encrypt fast enough, so you better not slack!

What kind of advice is that?! #speaker:You  #portrait:you_alarmed

Save your defence energy for later, I'm sure it will come in handy eventually. #speaker:Console  #portrait:console_evilexcited

Hey, what are you whispering about over there?! I don't have all day! #speaker:Guard Bot  #portrait:Default

->DONE

===battleEnd===
Good job on your first battle! You won! #speaker:Console  #portrait:console_evilexcited
Now let's get into the elevator before any more guard bots arrive.  
->DONE

===battleLost===
Uh oh, looks like you didn't defeat the guard bot! #speaker:Console  #portrait:console_wat
Alright, let's regroup and try fighting it again.
->DONE

/*
We'll have to be strategic about it. #speaker:Console  #portrait:console_neutral
This is a turn-based battle, so you and the robot take turns attacking and defending. 
You look like a weak little friend so your best bet is to avoid dying before it does.
Put all your effort into defence!

Alright, so how do I defend? #speaker:You  #portrait:you_confused
*/

-> END
