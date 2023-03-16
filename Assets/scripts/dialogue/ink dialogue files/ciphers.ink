//this is a draft that has not yet been fully proofread or fact checked

=== ciphers ===
Dammit, Long Dusk's communications are all encrypted! #speaker:Console #portrait:console_neutral #layout:layout_them

What does that mean? #speaker:You #portrait:you_puzzled #layout:layout_you

{"*sigh*"} Looks like it's time to teach you about ciphers. #speaker:Console #layout:layout_them #portrait:console_neutral
Ciphers encode text in a way that makes it impossible for someone to read it without the secret key.
This makes comunication secure, so you can share your secrets without anyone learning them.

Does that mean we will never be able to learn Dusk's secrets? I guess that's mission failed, we'll get 'em next time... #speaker:You #layout:layout_you #portrait:you_sad

Not so fast! It looks like these robots use a substitution cipher called Caesar cipher for their messages. #speaker:Console #layout:layout_them #portrait:console_evilexcited

Is that good? #speaker:You #layout:layout_you #portrait:you_neutral

Yeah, you're in luck - this cipher is quite easy to break! #speaker:Console #layout:layout_them #portrait:console_evilexcited
Let me explain some cipher terminology to you first - you have to know your enemy before you can defeat it. #portrait:console_neutral
#puzzle:caesarCipherDemo

Oh! #speaker:You #layout:layout_you #portrait:you_alarmed

This screen should help you understand the cipher. #speaker:Console #layout:layout_them #portrait:console_neutral
Feel free to play around with it using your arrow keys.
Quick, give me a sentence! #portrait:console_evilexcited

Uh..! What kind of sentenc- #speaker:You #layout:layout_you #portrait:you_alarmed

Perfect! Now, the message "What kind of sentence?" is currently not encoded, and we call that PLAINTEXT. #speaker:Console #layout:layout_them #portrait:console_evilexcited

W-wait! I didn't mea- #speaker:You #layout:layout_you #portrait:you_alarmed

...Now we can pick a random number between 1 and 26. This will be our ENCRYPTION KEY. Let's say the key is 4. #speaker:Console #layout:layout_them #portrait:console_evilexcited

Why is it between 1 and 26? Can we not change that range? #speaker:You #layout:layout_you #portrait:you_puzzled 

That's because this cipher - the Caesar cipher - works by shifting the alphabet by a certain number of letters.  #speaker:Console #layout:layout_them #portrait:console_neutral
Since there are 26 letters in the english alphabet, a key of more than 26 will wrap around back to the start of the alphabet. 
This will be equivalent to starting counting from 0 again, so it makes no sense to have a key bigger than 26.
Also, a key of 0 is the same as not encrypting the plaintext, since the plaintext and ciphertext 
alphabets will be the same, in oterwords there will be no offset. 
Therefore the key must be above 0.

So how do we use this key then? #speaker:You #layout:layout_you #portrait:you_puzzled 

The encryption key will encode the plaintext into gibberish that no one can understand. #speaker:Console #layout:layout_them #portrait:console_neutral
The encoding process is called ENCRYPTION and the resulting encoded text is called CIPHERTEXT.
Since our key is 4, to encrypt the plaintext we need to replace each plaintext letter with the letter 4 places to the right from it in the 
alphabet.
For example, the letter A will be relaced with E, because E is 4 places down the alphabet.

But the first letter of our plaintext is W!  W - X - Y - Z, that's only 3 hops and the alphabet has ended. How will that  #speaker:You #layout:layout_you #portrait:you_puzzled
work?

This is where the wrapping comes into play! We treat the alphabet like a loop rather than a finite line, so once you reach the end  #speaker:Console #layout:layout_them #portrait:console_neutral
you hop right back to the start.

Does that mean that W will become A because A is at the start of the alphabet? #speaker:You #layout:layout_you #portrait:you_puzzled

Yes, exactly! Because we only needed to hop one more place after Z, the letter W will map to the letter A. If you needed to encode 
the letter Y with the key of 4, which letter will it become? #speaker:Console #layout:layout_them #portrait:console_neutral
...
* [A] 
    -> ciphers.wrong("A")
* [B] 
    -> ciphers.wrong("B")
* [C]
    -> ciphers.correct("C")
    //don't actually have to do the dot notation, it just knows

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
I will spare you the trouble and just tell you the ciphertext myself: it's "Alex omrh sj wirxirgi". #portrait:console_evilexcited

Huh? It has the name Alex in it, what does that mean? #speaker:You #layout:layout_you #portrait:you_puzzled

Haha, it doesn't actually mean anything. It just so happened that encrypting the word "What" with a key of 4 transformed it into #speaker:Console #layout:layout_them #portrait:console_evilexcited
what looks like a real word. 
#puzzle:closePuzzle
You will likely find lots of other plaintexts and keys that result in curious ciphertexts on your encryption journey. #portrait:console_neutral
Now let's intercept a message from one of the robots and I will teach you how to break this cipher!  #portrait:console_evilexcited

What?! That sounds illegal! #speaker:You #layout:layout_you #portrait:you_alarmed

You only NOW you realised what we're doing? Too late to back out, we're in our criminal era, teehee #speaker:Console #layout:layout_them #portrait:console_evilexcited
-> DONE

=== decrypt ===
Looks like this robot is talking to the door. #speaker:Console #layout:layout_them #portrait:console_neutral
Let's see if we can send an encrypted message to the door pretending to be the robot asking to open it.

Don't we need to know the encryption key that the robot and door share if we want to pretend to be the robot? #speaker:You #layout:layout_you #portrait:you_puzzled

That's correct, we will first have to figure out the key. #speaker:Console #layout:layout_them #portrait:console_neutral
Let's listen in on what the robot is saying! #portrait:console_evilexcited

//make the key be randomly selected for the same text each playthrough, eg through game manager?
Tqk paad, mdq kag rdqq fayaddai mrfqd euj by? U ime ftuzwuzs iq oagxp tmzs agf, ur kag wzai itmf U yqmz. #speaker:Robot #layout:layout_them #portrait:robot_smug

That sounds like a different language! How will we manage to find the right key to decipher this? #speaker:You #layout:layout_you #portrait:you_puzzled

We could perform a brute force attack! #speaker:Console #layout:layout_them #portrait:console_evilexcited

That sounds violent! #speaker:You #layout:layout_you #portrait:you_alarmed

Haha, no, there will not be any physical violence involved. #speaker:Console #layout:layout_them #portrait:console_neutral
A BRUTE FORCE ATTACK uses trial and error to find the correct answer, in this case the encryption key.
We will try to use every possible key to find the correct key for this cipher, and decrypt the message. 
If the resulting plaintext looks like a normal sentence, then we know we found the key.

I think I understand that, but what exactly does decryption mean? #speaker:You #layout:layout_you #portrait:you_puzzled

DECRYPTION is simply converting ciphertext to plaintext. In other words, it's another word for decoding an encoded message. #speaker:Console #layout:layout_them #portrait:console_neutral
Together, an encryption algorithm and a decryption algorithm make up a CIPHER. 
They both share one key.
For a brute force attack, we don't need to know the Caesar Cipher decryption algorithm, so I will tell you about it later. #portrait:console_evilexcited

Alright, so how do we perform this attack? #speaker:You #layout:layout_you #portrait:you_puzzled

First, you will need to remember how many possible keys an English Caesar cipher has. #speaker:Console #layout:layout_them #portrait:console_neutral

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
{answer}? Yes, that's true!  #portrait:console_evilexcited
-> continue

=continue
Because the Caesar cipher performs alphabetic shifts, it has as many possible keys as letters in the English alphabet.  #portrait:console_neutral
Remember, you can't shift a letter by more steps than the number of letters in the alphabet! Otherwise it will just wrap back to 0
and start again!
#puzzle:bruteForcePuzzle

Oh! What's this? #speaker:You #layout:layout_you #portrait:you_puzzled

It's the Bruce wheel! It's short for brute force wheel. I just made that name up. #speaker:Console #layout:layout_them #portrait:console_evilexcited

So what does it do? #speaker:You #layout:layout_you #portrait:you_puzzled

You can use the left and right arrow keys to rotate the wheel. #speaker:Console #layout:layout_them #portrait:console_neutral
Each sector on the wheel represents a Caesar cipher key.
You can perform a brute force attack by trying out each key on the wheel until the message looks like real words.
That will mean you found the right key.

Alright, I'll give it a try. #speaker:You #layout:layout_you #portrait:you_happy


//plaintext: Hey door, are you free tomorrow after six pm? I was thinking we could hang out, if you know what I mean.

->DONE


=== bruteForceMinigame ====
Good job on your very first brute force attack! I'll make you into a real cyber criminal in no time! #speaker:Console #layout:layout_them #portrait:console_evilexcited

W-what?! #speaker:You #layout:layout_you #portrait:you_alarmed
Well I guess I can't deny that we're doing something illegal... #portrait:you_neutral

So, from the brute force attack we now know that the message was encrypted with the key of 12. #speaker:Console #layout:layout_them #portrait:console_neutral
This means that every letter in the original message was shifted to the letter 12 places away from it in the alphabet.
#puzzle:closePuzzle
We can now try to encrypt our own message and send it to the door to open it.

-> DONE

//ideas for the future:
//have a sequence where we have to decrypt a message to showcase reversible keys
//explain why brute force is not always efficent - this ciper only has 26 keys but real ciphers can have millions and billions and trillions which takes much much longer to crack
//frequency analys attack
//explain substitution cipher vs caesar cipher - finall boss-type puzzle
//why ciphers are needed in irl - encryption of internet traffic (online messaging, online banking, transactions)

=== encrypt ===

 -> DONE
 
 === battle ===
 Wait, you are not authorised to be here!  #speaker:Guard Bot #layout:layout_them #portrait:bot
 Our state-of-the-art security system will ensure you will never get through this door!
 
 Uh-oh, guess it's time to put what we learned to the test! #speaker:Console #layout:layout_them #portrait:console_evilexcited
 
 #puzzle:battle
 -> DONE
 
 
 -> END
